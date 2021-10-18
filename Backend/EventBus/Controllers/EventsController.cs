using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Entities.Models;
using EventBus.Data;
using HttpClients.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EventBus.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IDataContext _dataContext;

        public EventsController(HttpClient httpClient, IDataContext dataContext)
        {
            _httpClient = httpClient;
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessEvents([FromBody] Event eventModel)
        {
            _dataContext.Events.Add(eventModel);
            
            var eventListenersUrls = new[]
            {
                "http://localhost:5000/api/events",
                "http://localhost:5001/api/events",
                "http://localhost:5003/api/events",
                "http://localhost:5004/api/events",
            };

            await Task.WhenAll(eventListenersUrls.Select(async url =>
            {
                try
                {
                    var message = HttpHelpers.GetJsonMessage(url, HttpMethod.Post, eventModel, UriKind.Absolute);
                    await _httpClient.SendAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to send event to {url}");
                }
            }));

            Console.WriteLine($"--> Event {eventModel.Type} was sent to other services");

            return Ok();
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            Console.WriteLine($"Sending events: {JsonSerializer.Serialize(_dataContext.Events)}");
            
            return Ok(_dataContext.Events);
        }
    }
}