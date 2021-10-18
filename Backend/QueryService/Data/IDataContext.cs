using System.Collections.Generic;
using Entities.Models;

namespace QueryService.Data
{
    public interface IDataContext
    {
        public IList<Post> Posts { get; set; }
    }
}