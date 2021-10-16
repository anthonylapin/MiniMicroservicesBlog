using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClients.Helpers;
using HttpClients.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventBus.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        
        public EventsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        [HttpPost]
        public async Task<IActionResult> ProcessEvents([FromBody] EventModel eventModel)
        {
            var eventListenersUrls = new[]
            {
                "http://localhost:5000/api/events",
                "http://localhost:5001/api/events",
                "http://localhost:5003/api/events"
            };
            
            await Task.WhenAll(eventListenersUrls.Select(url =>
            {
                var message = HttpHelpers.GetJsonMessage(url, HttpMethod.Post, eventModel, UriKind.Absolute);
                return _httpClient.SendAsync(message);
            }));
            
            Console.WriteLine($"--> Event {eventModel.Type} was sent to other services");

            return Ok();
        }
    }
}