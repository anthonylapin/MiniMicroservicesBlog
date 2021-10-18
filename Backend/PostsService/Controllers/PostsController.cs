using System;
using System.Linq;
using System.Threading.Tasks;
using Entities.Enum;
using Entities.Models;
using HttpClients;
using Microsoft.AspNetCore.Mvc;
using PostsService.Data;
using PostsService.DataTransferObjects;

namespace PostsService.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly IEventBusClient _eventBusClient;

        public PostsController(IDataContext dataContext, IEventBusClient eventBusClient)
        {
            _dataContext = dataContext;
            _eventBusClient = eventBusClient;
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok(_dataContext.Posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {
            var lastPost = _dataContext.Posts.OrderByDescending(p => p.Id).FirstOrDefault();

            var post = new Post
            {
                Id = lastPost != null ? lastPost.Id + 1 : 0,
                Title = postDto.Title
            };

            _dataContext.Posts.Add(post);

            await _eventBusClient.SendEvent(EventTypes.PostsCreate, post);

            return Created(new Uri("api/posts", UriKind.Relative), post);
        }
    }
}