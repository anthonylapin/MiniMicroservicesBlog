using System.Collections.Generic;
using QueryService.Models;

namespace QueryService.Data
{
    public class DataContext : IDataContext
    {
        public IList<Post> Posts { get; set; }

        public DataContext()
        {
            Posts = new List<Post>();
        }
    }
}