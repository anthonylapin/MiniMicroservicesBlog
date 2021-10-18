using System;
using System.Text.Json;
using System.Threading.Tasks;
using Entities.Enum;
using Entities.Models;
using HttpClients;
using HttpClients.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ModerationService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventBusClient _eventBusClient;

        public EventsController(IEventBusClient eventBusClient)
        {
            _eventBusClient = eventBusClient;
        }
        
        [HttpPost]
        public async Task<IActionResult> ProcessEvents(Event eventModel)
        {
            Console.WriteLine($"--> Event received in Moderation Service: {JsonSerializer.Serialize(eventModel)}");

            switch (eventModel.Type)
            {
                case EventTypes.CommentsCreate:
                    await ModerateComment(eventModel);
                    break;
                default:
                    Console.WriteLine($"No event handler for this type of event: {eventModel.Type}");
                    break;
            }

            return Ok();
        }

        private async Task ModerateComment(Event eventModel)
        {
            var comment = JsonHelpers.DeserializeEventPayload<Comment>(eventModel);

            comment.CommentStatus = comment.Content.Contains("orange") 
                ? CommentStatuses.Rejected 
                : CommentStatuses.Approved;

            await _eventBusClient.SendEvent(EventTypes.CommentModerated, comment);
        }
    }
}