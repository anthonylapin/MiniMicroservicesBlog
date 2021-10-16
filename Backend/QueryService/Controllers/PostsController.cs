using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using QueryService.Data;

namespace QueryService.Controllers
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
            Console.WriteLine($"--> Processing GetAllPosts request. Return Posts: {JsonSerializer.Serialize(_dataContext.Posts)}");
            return Ok(_dataContext.Posts);
        }
    }
}