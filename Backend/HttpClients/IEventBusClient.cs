using System.Threading.Tasks;
using HttpClients.Enum;

namespace HttpClients
{
    public interface IEventBusClient
    {
        Task SendEvent(EventTypes type, object payload);
    }
}