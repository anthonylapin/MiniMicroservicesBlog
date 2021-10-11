using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PostsService.Data;
using PostsService.DataTransferObjects;
using PostsService.Models;

namespace PostsService.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IDataContext _dataContext;

        public PostsController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok(_dataContext.Posts);
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostDto postDto)
        {
            var lastPost = _dataContext.Posts.OrderByDescending(p => p.Id).FirstOrDefault();

            var post = new Post
            {
                Id = lastPost != null ? lastPost.Id + 1 : 0,
                Title = postDto.Title
            };

            _dataContext.Posts.Add(post);

            return Ok(post);
        }
    }
}