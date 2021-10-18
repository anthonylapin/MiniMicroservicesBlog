using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Entities.Enum;
using Entities.Models;
using HttpClients.Helpers;
using HttpClients.Options;
using Microsoft.Extensions.Options;

namespace HttpClients
{
    public class EventBusClient : IEventBusClient
    {
        private const string RelativeEventBusUrl = "/api/events";
        
        private readonly HttpClient _httpClient;

        public EventBusClient(HttpClient httpClient, IOptions<EventBusClientOptions> eventBusClientOptions)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(eventBusClientOptions.Value.BaseUrl);
        }

        public async Task SendEvent(EventTypes type, object payload)
        {
            Console.WriteLine($"--> Prepare to send event of type {type} to event bus.");

            var eventModel = new Event
            {
                Type = type,
                Payload = JsonSerializer.Serialize(payload)
            };

            var httpRequestMessage =
                HttpHelpers.GetJsonMessage(RelativeEventBusUrl, HttpMethod.Post, eventModel, UriKind.Relative);

            try
            {
                var response =
                    await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead);

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

        public async Task<IList<Event>> GetEvents()
        {
            Console.WriteLine($"--> Prepare to get events from event bus.");

            var httpMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(RelativeEventBusUrl, UriKind.Relative)
            };

            using var response = await _httpClient.SendAsync(httpMessage);

            var content = await response.Content.ReadAsStringAsync();
            
            var events = JsonSerializer.Deserialize<Event[]>(content);

            return events;
        }
    }
}