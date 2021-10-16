using System.Collections.Generic;
using QueryService.Models;

namespace QueryService.Data
{
    public interface IDataContext
    {
        public IList<Post> Posts { get; set; }
    }
}