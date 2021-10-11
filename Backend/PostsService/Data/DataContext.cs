using System;
using System.Collections.Generic;
using PostsService.Models;

namespace PostsService.Data
{
    public class DataContext : IDataContext
    {
        public IList<Post> Posts { get; }

        public DataContext()
        {
            Posts = new List<Post>();
        }
    }
}