using System.Collections.Generic;
using Entities.Models;

namespace QueryService.Data
{
    public class DataContext : IDataContext
    {
        public DataContext()
        {
            Posts = new List<Post>();
        }

        public IList<Post> Posts { get; set; }
    }
}