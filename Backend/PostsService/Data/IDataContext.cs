using System.Collections.Generic;
using PostsService.Models;

namespace PostsService.Data
{
    public interface IDataContext
    {
        IList<Post> Posts { get; }
    }
}