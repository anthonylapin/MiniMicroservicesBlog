using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommentsService.Data;
using Entities.Enum;
using Entities.Models;
using HttpClients;
using HttpClients.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CommentsService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private IDataContext _dataContext;
        private IEventBusClient _eventBusClient;

        public EventsController(IDataContext dataContext, IEventBusClient eventBusClient)
        {
            _dataContext = dataContext;
            _eventBusClient = eventBusClient;
        }
        
        [HttpPost]
        public async Task<IActionResult> ReceiveEvent(Event eventModel)
        {
            Console.WriteLine($"--> Event received in comments service: {JsonSerializer.Serialize(eventModel)}");

            switch (eventModel.Type)
            {
                case EventTypes.CommentModerated:
                    await HandleUpdateComment(eventModel);
                    break;
                default:
                    Console.WriteLine($"--> No event handlers for this type of event created: {eventModel.Type}");
                    break;
            }

            return Ok();
        }

        private async Task HandleUpdateComment(Event eventModel)
        {
            var payload = JsonHelpers.DeserializeEventPayload<Comment>(eventModel);

            var comment = _dataContext.Comments[payload.PostId].FirstOrDefault(c => c.Id == payload.Id);

            if (comment == null)
            {
                throw new InvalidDataException();
            }

            comment.CommentStatus = payload.CommentStatus;

            await _eventBusClient.SendEvent(EventTypes.CommentUpdated, comment);
        }
    }
}