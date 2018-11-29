using Microsoft.AspNetCore.Builder;
using RevolutionGolf.EventRequestBroker.EventServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevolutionGolf.EventRequestBroker.Extensions
{
    public static class EventRequestBrokerExtensions
    {
        public static IApplicationBuilder UseEventRequestBroker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EventRequestBroker>();
        }

        public static IApplicationBuilder UseEventRequestBroker<TService>(this IApplicationBuilder builder, TService service)
            where TService : class
        {
            return builder.UseMiddleware<EventRequestBroker>(service);
        }

        public static IApplicationBuilder UseEventRequestBroker<TService>(this IApplicationBuilder builder)
            where TService : new()
        {
            return builder.UseMiddleware<EventRequestBroker>(new TService());
        }

        public static IApplicationBuilder UseEventRequestBroker<TService, TImplementation>(this IApplicationBuilder builder, TImplementation service) 
            where TService : class
            where TImplementation : class, TService
        {
            return builder.UseMiddleware<EventRequestBroker>(service);
        }

        public static IApplicationBuilder UseEventRequestBroker<TService, TImplementation>(this IApplicationBuilder builder)
            where TService : class
            where TImplementation : TService, new()
        {
            return builder.UseMiddleware<EventRequestBroker>(new TImplementation());
        }
    }
}
