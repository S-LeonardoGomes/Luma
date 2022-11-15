using LumaEventService.Models.DTO;

namespace LumaEventService.Services
{
    public interface IEventService
    {
        IEnumerable<ReadEventDTO> GetAllEvents(string? loggedInUsername);
        IEnumerable<ReadEventDTO> GetNextEvents(string? loggedInUsername);
        ReadEventDTO GetEventById(string eventId, string? loggedInUsername);
        void AddNewEvent(ReadEventDTO userEvent, string? loggedInUsername);
        ReadEventDTO UpdateEvent(string eventId, ReadEventDTO userEvent, string? loggedInUsername);
        ReadEventDTO RemoveEvent(string eventId, string? loggedInUsername);
    }
}
