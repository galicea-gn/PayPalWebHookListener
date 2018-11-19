using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
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

            if (headerObject == null ||
                !await _authenticationService.IsValidRequest(headerObject, await new StreamReader(Request.Body).ReadToEndAsync(), Options.WebHookId))
            {
                return AuthenticateResult.Fail("Invalid transmission signature!");
            }

            return AuthenticateResult.Success(null);
        }

        private HeaderObject GetHeaderObject()
        {
            if (!Request.Headers.ContainsKey(HeaderConstants.TransmissionSig) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.TransmissionSig]))
            {
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.TransmissionId) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.TransmissionId]))
            {
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.TransmissionTime) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.TransmissionTime]))
            {
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.AuthenticationAlogrithm) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.AuthenticationAlogrithm]))
            {
                return null;
            }
            else if (!Request.Headers.ContainsKey(HeaderConstants.CertificateUrl) &&
                string.IsNullOrEmpty(Request.Headers[HeaderConstants.CertificateUrl]))
            {
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
        public static readonly string TransmissionSig = "PAYPAL-TRANSMISSION-SIG";
        public static readonly string TransmissionId = "PAYPAL-TRANSMISSION-ID";
        public static readonly string TransmissionTime = "PAYPA-TRANSMISSION-TIME";
        public static readonly string AuthenticationAlogrithm = "PAYPAL-AUTH-ALGO";
        public static readonly string CertificateUrl = "PAYPAL-CERT-URL";
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
