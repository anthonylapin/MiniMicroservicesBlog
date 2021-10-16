using System.Text.Json.Serialization;
using HttpClients.Enum;

namespace HttpClients.Models
{
    public class EventModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventTypes Type { get; set; }
        public string Payload { get; set; }
    }
}