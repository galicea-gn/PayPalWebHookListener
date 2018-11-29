using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RevolutionGolf.EventRequestBroker.Middleware;
using RevolutionGolf.EventRequestBroker.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevolutionGolf.EventRequestBroker.Extensions
{
    public static class EventRequestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddPayPalWebHook<TAuthService>(this AuthenticationBuilder builder)
        where TAuthService : class, IEventRequestAuthenticationService
        {
            return AddPayPalWebHook<TAuthService>(builder, EventRequestAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddPayPalWebHook<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
            where TAuthService : class, IEventRequestAuthenticationService
        {
            return AddPayPalWebHook<TAuthService>(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddPayPalWebHook<TAuthService>(this AuthenticationBuilder builder, Action<EventRequestAuthenticationSchemeOptions> configureOptions)
            where TAuthService : class, IEventRequestAuthenticationService
        {
            return AddPayPalWebHook<TAuthService>(builder, EventRequestAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddPayPalWebHook<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<EventRequestAuthenticationSchemeOptions> configureOptions)
            where TAuthService : class, IEventRequestAuthenticationService
        {
            builder.Services.AddSingleton<IPostConfigureOptions<EventRequestAuthenticationSchemeOptions>, EventRequestAuthenticationSchemePostConfigureOptions>();
            builder.Services.AddTransient<IEventRequestAuthenticationService, TAuthService>();

            return builder.AddScheme<EventRequestAuthenticationSchemeOptions, EventRequestAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }
    }
}
