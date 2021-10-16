using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommentsService.Data;
using CommentsService.DataTransferObjects;
using CommentsService.Models;
using HttpClients;
using HttpClients.Dto;
using HttpClients.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CommentsService.Controllers
{
    [ApiController]
    [Route("api/posts/{postId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly IEventBusClient _eventBusClient;

        public CommentsController(IDataContext dataContext, IEventBusClient eventBusClient)
        {
            _dataContext = dataContext;
            _eventBusClient = eventBusClient;
        }
        
        [HttpGet]
        public IActionResult GetPostComments(int postId)
        {
            var commentsToPostDictionary = _dataContext.Comments;

            var comments = commentsToPostDictionary.ContainsKey(postId)
                ? commentsToPostDictionary[postId]
                : new List<Comment>();
            
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentToPost(int postId, [FromBody] AddCommentDto commentDto)
        {
            var commentId = Guid.NewGuid();

            var comment = new Comment
            {
                Id = commentId,
                Content = commentDto.Content,
            };

            IList<Comment> comments;

            if (_dataContext.Comments.ContainsKey(postId))
            {
                comments = _dataContext.Comments[postId];
                comments.Add(comment);
            }
            else
            {
                comments = new List<Comment> {comment};
                _dataContext.Comments[postId] = comments;
            }

            var commentCreatedPayload = new CommentAddedDto
            {
                PostId = postId,
                Id = comment.Id,
                Content = comment.Content
            };

            await _eventBusClient.SendEvent(EventTypes.CommentsCreate, commentCreatedPayload);
            
            return Ok(comments);
        }
    }
}