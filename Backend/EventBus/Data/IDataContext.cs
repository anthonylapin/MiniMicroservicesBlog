using System.Collections.Generic;
using Entities.Models;

namespace EventBus.Data
{
    public interface IDataContext
    {
        IList<Event> Events { get; set; }
    }
}