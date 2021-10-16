using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HttpClients.Enum;
using HttpClients.Helpers;
using HttpClients.Models;
using HttpClients.Options;
using Microsoft.Extensions.Options;

namespace HttpClients
{
    public class EventBusClient : IEventBusClient
    {
        private readonly HttpClient _httpClient;

        public EventBusClient(HttpClient httpClient, IOptions<EventBusClientOptions> eventBusClientOptions)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(eventBusClientOptions.Value.BaseUrl);
        }
        
        public async Task SendEvent(EventTypes type, object payload)
        {
            Console.WriteLine($"--> Prepare to send event of type {type} to event bus.");
            
            const string relativeUrl = "/api/events";
            
            var eventModel = new EventModel
            {
                Type = type,
                Payload = JsonSerializer.Serialize(payload)
            };

            var httpRequestMessage =
                HttpHelpers.GetJsonMessage(relativeUrl, HttpMethod.Post, eventModel, UriKind.Relative);

            try
            {
                var response = await _httpClient.SendAsync(httpRequestMessage,  HttpCompletionOption.ResponseContentRead);

                Console.WriteLine(response.IsSuccessStatusCode
                    ? $"--> Event {eventModel.Type} was sent to event bus."
                    : $"--> Event {eventModel.Type} was not sent to event bus. StatusCode: {response.StatusCode}. Response: {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Event {eventModel.Type} was not sent to event bus. Error: {e.Message}");
                throw;
            }
            
        }
    }
}