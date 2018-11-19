using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RevolutionGolf.EventDto
{
    public class NotificationMessageDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("create_time")]
        public string CreateTime { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("resource")]
        public object Resource { get; set; }

        public Type TypedResourceType { get; set; }

        [JsonProperty("links")]
        public IEnumerable<LinkDto> Links { get; set; }
    }

    public class LinkDto
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
