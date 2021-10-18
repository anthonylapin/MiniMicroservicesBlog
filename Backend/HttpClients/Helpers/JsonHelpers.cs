using System;
using System.Text.Json;
using Entities.Models;

namespace HttpClients.Helpers
{
    public static class JsonHelpers
    {
        public static T DeserializeEventPayload<T>(Event eventModel)
        {
            var payload = JsonSerializer.Deserialize<T>(eventModel.Payload);

            if (payload == null)
                throw new InvalidCastException($"Invalid argument was passed in {eventModel.Type} payload");

            return payload;
        }
    }
}