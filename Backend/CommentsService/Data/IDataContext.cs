using System.Collections.Generic;
using CommentsService.Models;

namespace CommentsService.Data
{
    public interface IDataContext
    {
        Dictionary<int, IList<Comment>> Comments { get; }
    }
}