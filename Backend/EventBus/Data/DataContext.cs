using System.Collections.Generic;
using Entities.Models;

namespace EventBus.Data
{
    public class DataContext : IDataContext
    {
        public IList<Event> Events { get; set; }

        public DataContext()
        {
            Events = new List<Event>();
        }
    }
}