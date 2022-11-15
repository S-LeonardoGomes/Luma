using LumaEventService.Models;

namespace LumaEventService.Data
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents(string? loggedInUsername);
        IEnumerable<Event> GetNextEvents(string? loggedInUsername);
        Event GetEventById(string eventId, string? loggedInUsername);
        void AddNewEvent(Event userEvent);
        void UpdateEvent(Event userEvent);
        void RemoveEvent(Event userEvent);
        Event GetEventByDateAndUser(Event userEvent, string? loggedInUsername);
    }
}
