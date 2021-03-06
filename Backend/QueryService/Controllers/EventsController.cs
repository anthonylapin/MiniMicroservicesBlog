using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Entities.Enum;
using Entities.Models;
using HttpClients.Helpers;
using Microsoft.AspNetCore.Mvc;
using QueryService.Data;

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
        public IActionResult ReceiveEvent(Event eventModel)
        {
            Console.WriteLine($"--> Event received in query service: {JsonSerializer.Serialize(eventModel)}");

            try
            {
                HandleEvent(eventModel);
                
                return Ok();
            }
            catch (Exception e)
            {
                var errorMessage = $"Error occurred while processing events: {e.Message}";

                Console.WriteLine($"--> {errorMessage}");

                return BadRequest(errorMessage);
            }
        }

        private void HandleEvent(Event eventModel)
        {
            switch (eventModel.Type)
            {
                case EventTypes.PostsCreate:
                    ProcessPostsCreateEvent(eventModel);
                    break;
                case EventTypes.CommentsCreate:
                    ProcessCommentCreateEvent(eventModel);
                    break;
                case EventTypes.CommentUpdated:
                    ProcessCommentUpdatedEvent(eventModel);
                    break;
                default:
                    Console.WriteLine($"--> No logic was created to process event: {eventModel.Type}");
                    break;
            }
        }

        private void ProcessCommentCreateEvent(Event eventModel)
        {
            var comment = JsonHelpers.DeserializeEventPayload<Comment>(eventModel);

            var post = _dataContext.Posts.FirstOrDefault(p => p.Id == comment.PostId);

            if (post == null) throw new InvalidDataException("Comment for non existent post was passed");

            post.Comments.Add(comment);

            Console.WriteLine($"--> New Comment {comment.Content} was added to a post {post.Title}.");
        }

        private void ProcessPostsCreateEvent(Event eventModel)
        {
            var post = JsonHelpers.DeserializeEventPayload<Post>(eventModel);

            _dataContext.Posts.Add(post);
            
            Console.WriteLine($"--> New Post was added: {post.Id} {post.Title}");
        }

        private void ProcessCommentUpdatedEvent(Event eventModel)
        {
            var payload = JsonHelpers.DeserializeEventPayload<Comment>(eventModel);

            var post = _dataContext.Posts.FirstOrDefault(p => p.Id == payload.PostId);

            if (post == null)
            {
                throw new InvalidDataException("Comment for non existent post was passed");
            }

            var comment = post.Comments.FirstOrDefault(c => c.Id == payload.Id);

            if (comment == null)
            {
                throw new InvalidDataException("Non existent comment was passed");
            }

            comment.CommentStatus = payload.CommentStatus;
            comment.Content = payload.Content;
        }
    }
}