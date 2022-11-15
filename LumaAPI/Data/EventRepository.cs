using LumaEventService.Data.Context;
using LumaEventService.Models;
using Microsoft.EntityFrameworkCore;

namespace LumaEventService.Data
{
    public class EventRepository : IEventRepository, IDisposable
    {
        private readonly EventServiceContext _context;

        public EventRepository(EventServiceContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAllEvents(string? loggedInUsername)
        {
            return _context.LumaEvents.AsNoTracking().Where(x => x.UserName == loggedInUsername);
        }

        public IEnumerable<Event> GetNextEvents(string? loggedInUsername)
        {
            return _context.LumaEvents.AsNoTracking().Where(x => x.EventUtcDateStart > DateTime.UtcNow && x.UserName == loggedInUsername);
        }

        public Event GetEventById(string eventId, string? loggedInUsername)
        {
            return _context.LumaEvents.AsNoTracking().Where(x => x.UserName == loggedInUsername).FirstOrDefault(x => x.EventId == eventId);
        }

        public void AddNewEvent(Event userEvent)
        {
            _context.Add(userEvent);
            _context.SaveChanges();
        }

        public void UpdateEvent(Event userEvent)
        {
            _context.LumaEvents.Update(userEvent);
            _context.Entry(userEvent).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RemoveEvent(Event userEvent)
        {
            _context.Remove(userEvent);
            _context.SaveChanges();
        }

        public Event GetEventByDateAndUser(Event userEvent, string? loggedInUsername)
        {
            return _context.LumaEvents.AsNoTracking().Where(x => x.UserName == loggedInUsername).FirstOrDefault(x =>
                (x.EventUtcDateStart <= userEvent.EventUtcDateStart && x.EventUtcDateEnd >= userEvent.EventUtcDateStart) ||
                (x.EventUtcDateStart <= userEvent.EventUtcDateEnd && x.EventUtcDateEnd >= userEvent.EventUtcDateEnd));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
