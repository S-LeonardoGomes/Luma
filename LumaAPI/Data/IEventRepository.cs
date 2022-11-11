using LumaEventService.Models;

namespace LumaEventService.Data
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents();
        IEnumerable<Event> GetNextEvents();
        Event GetEventById(string eventId);
        void AddNewEvent(Event userEvent);
        void UpdateEvent(Event userEvent);
        void RemoveEvent(Event userEvent);
        Event GetEventByDateAndUser(Event userEvent);
    }
}
