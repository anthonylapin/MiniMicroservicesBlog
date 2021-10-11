using System;
using System.Collections.Generic;
using CommentsService.Data;
using CommentsService.DataTransferObjects;
using CommentsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentsService.Controllers
{
    [ApiController]
    [Route("api/posts/{postId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public CommentsController(IDataContext dataContext)
        {
            _dataContext = dataContext;
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
        public IActionResult AddCommentToPost(int postId, [FromBody] AddCommentDto commentDto)
        {
            var commentId = Guid.NewGuid();

            var comment = new Comment
            {
                Id = commentId,
                Content = commentDto.Content
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
            
            return Ok(comments);
        }
    }
}