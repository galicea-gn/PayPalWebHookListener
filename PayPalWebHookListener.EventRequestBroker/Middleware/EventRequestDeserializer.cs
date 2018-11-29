using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RevolutionGolf.EventDto;
using RevolutionGolf.EventDto.Billing;
using RevolutionGolf.EventDto.Payments;
using RevolutionGolf.EventRequestBroker.Configuration;
using RevolutionGolf.EventRequestBroker.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RevolutionGolf.EventRequestBroker
{
    public class EventRequestDeserializer
    {
        private readonly EventRequestBrokerConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public EventRequestDeserializer(
            RequestDelegate next, 
            IOptions<EventRequestBrokerConfiguration> configuration,
            ILogger<EventRequestDeserializer> logger)
        {
            if (configuration == null || configuration.Value == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _next = next;
            _config = configuration.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Equals(_config.EndpointPath, StringComparison.Ordinal) &&
                httpContext.Request.Method == HttpMethods.Post)
            {
                if (!httpContext.User.HasClaim(x => x.Type == ClaimTypes.Authentication && x.Value == "true"))
                {
                    await httpContext.ChallengeAsync(EventRequestAuthenticationDefaults.AuthenticationScheme);
                }
                else
                {
                    string body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                    try
                    {
                        NotificationMessageDto message = JsonConvert.DeserializeObject<NotificationMessageDto>(body);
                        message.TypedResourceType = GetDtoTypeFromResource(message.ResourceType);

                        if (httpContext.Items.TryAdd("Message", message))
                        {
                            await _next.Invoke(httpContext);
                        }
                        else
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await httpContext.Response.WriteAsync($"Failed to deserialize message! Resource type was: {message.ResourceType}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{DateTime.Now}: Bad request encountered. Failed to deserialize message with error: {ex.Message}");
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await httpContext.Response.WriteAsync($"Bad request!");
                    }
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }

        private Type GetDtoTypeFromResource(string resource)
        {
            if (resource == ResourceTypeEnumeration.BillingAgreement)
            {
                return typeof(BillingAgreementDto);
            }
            else if (resource == ResourceTypeEnumeration.Authorization)
            {
                return typeof(AuthorizationDto);
            }
            return null;
        }
    }

    public static class ResourceTypeEnumeration
    {
        public const string BillingAgreement = "Agreement";
        public const string Authorization = "authorization";
    }
}
