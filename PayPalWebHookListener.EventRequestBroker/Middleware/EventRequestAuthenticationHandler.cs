using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RevolutionGolf.EventRequestBroker.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RevolutionGolf.EventRequestBroker.Middleware
{
    public class EventRequestAuthenticationHandler : AuthenticationHandler<EventRequestAuthenticationSchemeOptions>
    {
        private readonly IEventRequestAuthenticationService _authenticationService;

        public EventRequestAuthenticationHandler(
            IOptionsMonitor<EventRequestAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            IEventRequestAuthenticationService authenticationService,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            HeaderObject headerObject = GetHeaderObject();
            string body = await new StreamReader(Request.Body).ReadToEndAsync();

            if (headerObject == null ||
                !await _authenticationService.IsValidRequest(headerObject, await new StreamReader(Request.Body).ReadToEndAsync(), Options.WebHookId))
            {
                return AuthenticateResult.Fail("Invalid transmission signature!");
            }

            Request.Body.Position = 0;
            IEnumerable<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.Authentication, "true") };
            IEnumerable<ClaimsIdentity> identity = new List<ClaimsIdentity>() { new ClaimsIdentity(claims) { } };
            return AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(15),
                        IsPersistent = false,
                        AllowRefresh = false
                    }, 
                    EventRequestAuthenticationDefaults.AuthenticationScheme));
        }

        private HeaderObject GetHeaderObject()
        {
            if (!Request.Headers.ContainsKey(HeaderConstants.TransmissionSig) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.TransmissionSig]))
            {
                Logger.LogError($"{DateTime.Now}: Authentication Failed. {HeaderConstants.TransmissionSig} header not found!");
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.TransmissionId) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.TransmissionId]))
            {
                Logger.LogError($"{DateTime.Now}: Authentication Failed. {HeaderConstants.TransmissionId} header not found!");
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.TransmissionTime) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.TransmissionTime]))
            {
                Logger.LogError($"{DateTime.Now}: Authentication Failed. {HeaderConstants.TransmissionTime} header not found!");
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.AuthenticationAlogrithm) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.AuthenticationAlogrithm]))
            {
                Logger.LogError($"{DateTime.Now}: Authentication Failed. {HeaderConstants.AuthenticationAlogrithm} header not found!");
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.CertificateUrl) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.CertificateUrl]))
            {
                Logger.LogError($"{DateTime.Now}: Authentication Failed. {HeaderConstants.CertificateUrl} header not found!");
                return null;
            }

            return new HeaderObject()
            {
                TransmissionSig = Request.Headers[HeaderConstants.TransmissionSig],
                TransmissionId = Request.Headers[HeaderConstants.TransmissionId],
                TransmissionTime = Request.Headers[HeaderConstants.TransmissionTime],
                AuthenticationAlogrithm = Request.Headers[HeaderConstants.AuthenticationAlogrithm],
                CertificateUrl = Request.Headers[HeaderConstants.CertificateUrl]
            };
        }
    }

    internal static class HeaderConstants
    {
        public const string TransmissionSig = "PAYPAL-TRANSMISSION-SIG";
        public const string TransmissionId = "PAYPAL-TRANSMISSION-ID";
        public const string TransmissionTime = "PAYPAL-TRANSMISSION-TIME";
        public const string AuthenticationAlogrithm = "PAYPAL-AUTH-ALGO";
        public const string CertificateUrl = "PAYPAL-CERT-URL";
    }

    public class HeaderObject
    {
        public string TransmissionSig { get; set; }
        public string TransmissionId { get; set; }
        public string TransmissionTime { get; set; }
        public string AuthenticationAlogrithm { get; set; }
        public string CertificateUrl { get; set; }
    }
}
