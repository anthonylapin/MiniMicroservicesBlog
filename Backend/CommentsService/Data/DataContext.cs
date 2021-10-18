using System.Collections.Generic;
using Entities.Models;

namespace CommentsService.Data
{
    public class DataContext : IDataContext
    {
        public DataContext()
        {
            Comments = new Dictionary<int, IList<Comment>>();
        }

        public Dictionary<int, IList<Comment>> Comments { get; }
    }
}