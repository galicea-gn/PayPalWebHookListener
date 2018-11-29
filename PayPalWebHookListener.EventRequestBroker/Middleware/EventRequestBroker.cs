using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevolutionGolf.EventDto;
using RevolutionGolf.EventRequestBroker.Configuration;
using RevolutionGolf.EventRequestBroker.EventServices;
using RevolutionGolf.EventRequestBroker.Options;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RevolutionGolf.EventRequestBroker
{
    public class EventRequestBroker
    {
        private readonly EventRequestBrokerConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly IEventService _service;
        private readonly ILogger _logger;

        public EventRequestBroker(
            RequestDelegate next, 
            IEventService eventService,
            IOptions<EventRequestBrokerConfiguration> configuration,
            ILogger<EventRequestBroker> logger)
        {
            if (configuration == null || configuration.Value == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _next = next;
            _service = eventService;
            _config = configuration.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Equals(_config.EndpointPath, StringComparison.OrdinalIgnoreCase) &&
                httpContext.Request.Method == HttpMethods.Post)
            {
                if (!httpContext.User.HasClaim(x => x.Type == ClaimTypes.Authentication && x.Value == "true"))
                {
                    await httpContext.ChallengeAsync(EventRequestAuthenticationDefaults.AuthenticationScheme);
                }
                else
                {
                    httpContext.Items.TryGetValue("Message", out object message);

                    if (message is NotificationMessageDto notificationMessage)
                    {
                        if (_service.CanConsumeMessage(notificationMessage.TypedResourceType))
                        {
                            _logger.LogTrace($"{DateTime.Now}: Found EventHandler for event type: {notificationMessage.EventType} and resource type: {notificationMessage.ResourceType}");
                            HttpStatusCode status = await _service.HandleEvent(notificationMessage);

                            _logger.LogInformation($"{DateTime.Now}: Event handled with status code: {status} for event type: {notificationMessage.EventType}");
                            httpContext.Response.StatusCode = (int)status;
                            await httpContext.Response.WriteAsync(string.Empty);
                        }
                        else
                        {
                            await _next.Invoke(httpContext);
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"{DateTime.Now}: Bad request encountered at path {httpContext.Request.Path}");
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await httpContext.Response.WriteAsync($"Failed to deserialize message!");
                    }
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}
