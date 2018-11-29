using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevolutionGolf.EventRequestBroker.Options
{
    public class EventRequestAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string WebHookId { get; set; }
    }

    public class EventRequestAuthenticationSchemePostConfigureOptions : IPostConfigureOptions<EventRequestAuthenticationSchemeOptions>
    {
        public void PostConfigure(string name, EventRequestAuthenticationSchemeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            else if (string.IsNullOrEmpty(options.WebHookId))
            {
                throw new InvalidOperationException("WebHookId must be provided in options!");
            }
        }
    }

    public static class EventRequestAuthenticationDefaults
    {
        public const string AuthenticationScheme = "PayPalWebHook";
        public const string TrustedFileCertKeyBundleLocation = "PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.pfx";
    }
}
