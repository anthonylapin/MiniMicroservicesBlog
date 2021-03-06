using System.Collections.Generic;
using Entities.Models;

namespace CommentsService.Data
{
    public interface IDataContext
    {
        Dictionary<int, IList<Comment>> Comments { get; }
    }
}