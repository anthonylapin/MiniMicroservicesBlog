using Entities.Models;

namespace QueryService.Util
{
    public interface IEventHandler
    {
        void HandleEvent(Event eventModel);
    }
}