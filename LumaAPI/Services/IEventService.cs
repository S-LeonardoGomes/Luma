using LumaEventService.Models.DTO;

namespace LumaEventService.Services
{
    public interface IEventService
    {
        IEnumerable<ReadEventDTO> GetAllEvents();
        IEnumerable<ReadEventDTO> GetNextEvents();
        ReadEventDTO GetEventById(string eventId);
        void AddNewEvent(ReadEventDTO userEvent);
        ReadEventDTO UpdateEvent(string eventId, ReadEventDTO userEvent);
        ReadEventDTO RemoveEvent(string eventId);
    }
}
