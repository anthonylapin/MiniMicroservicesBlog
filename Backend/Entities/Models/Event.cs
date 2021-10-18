using System.Text.Json.Serialization;
using Entities.Enum;

namespace Entities.Models
{
    public class Event
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventTypes Type { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }
}