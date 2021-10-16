using System;
using System.Text.Json;
using HttpClients.Models;
using Microsoft.AspNetCore.Mvc;

namespace PostsService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceiveEvent(EventModel eventModel)
        {
            Console.WriteLine($"--> Event received in posts service: {JsonSerializer.Serialize(eventModel)}");
            
            return Ok();
        }
    }
}