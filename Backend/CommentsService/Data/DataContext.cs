using System.Collections.Generic;
using CommentsService.Models;

namespace CommentsService.Data
{
    public class DataContext : IDataContext
    {
        public Dictionary<int, IList<Comment>> Comments { get; }

        public DataContext()
        {
            Comments = new Dictionary<int, IList<Comment>>();
        }
    }
}