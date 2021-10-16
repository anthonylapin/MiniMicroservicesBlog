using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using HttpClients.Enum;
using HttpClients.Models;
using Microsoft.AspNetCore.Mvc;
using QueryService.Data;
using QueryService.Models;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public EventsController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpPost]
        public IActionResult ReceiveEvent(EventModel eventModel)
        {
            Console.WriteLine($"--> Event received in query service: {JsonSerializer.Serialize(eventModel)}");

            try
            {
                switch (eventModel.Type)
                {
                    case EventTypes.PostsCreate:
                        ProcessPostsCreateEvent(eventModel);                    
                        break;
                    case EventTypes.CommentsCreate:
                        ProcessCommentCreateEvent(eventModel);
                        break;
                    default:
                        Console.WriteLine($"--> No logic was created to process event: {eventModel.Type}");
                        break;
                }
                
                return Ok();
            }
            catch (Exception e)
            {
                var errorMessage = $"Error occurred while processing events: {e.Message}";
                
                Console.WriteLine($"--> {errorMessage}");
                
                return BadRequest(errorMessage);
            }
        }

        private T DeserializeEventPayload<T>(EventModel eventModel)
        {
            var payload = JsonSerializer.Deserialize<T>(eventModel.Payload);
            
            if (payload == null)
            {
                throw new InvalidCastException($"Invalid argument was passed in {eventModel.Type} payload");
            }

            return payload;
        } 

        private void ProcessCommentCreateEvent(EventModel eventModel)
        {
            var comment = DeserializeEventPayload<Comment>(eventModel);

            var post = _dataContext.Posts.FirstOrDefault(p => p.Id == comment.PostId);

            if (post == null)
            {
                throw new InvalidDataException("Comment for non existent post was passed");
            }
            
            post.Comments.Add(comment);

            Console.WriteLine($"--> New Comment {comment.Content} was added to a post {post.Title}.");
        }

        private void ProcessPostsCreateEvent(EventModel eventModel)
        {
            var post = DeserializeEventPayload<Post>(eventModel);
           
            _dataContext.Posts.Add(post);
            Console.WriteLine($"--> New Post was added: {post.Id} {post.Title}");  
        }
    }
}