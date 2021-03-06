using System.Collections.Generic;
using Entities.Models;

namespace PostsService.Data
{
    public class DataContext : IDataContext
    {
        public DataContext()
        {
            Posts = new List<Post>();
        }

        public IList<Post> Posts { get; }
    }
}