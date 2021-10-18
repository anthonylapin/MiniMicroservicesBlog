using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Enum;
using Entities.Models;

namespace HttpClients
{
    public interface IEventBusClient
    {
        Task SendEvent(EventTypes type, object payload);
        Task<IList<Event>> GetEvents();
    }
}