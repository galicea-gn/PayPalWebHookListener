using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevolutionGolf.EventRequestBroker.Extensions
{
    public static class EventRequestDeserializerExtensions
    {
        public static IApplicationBuilder UseEventRequestDeserialization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EventRequestDeserializer>();
        }
    }
}
