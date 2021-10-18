using System.Collections.Generic;
using Entities.Models;

namespace PostsService.Data
{
    public interface IDataContext
    {
        IList<Post> Posts { get; }
    }
}