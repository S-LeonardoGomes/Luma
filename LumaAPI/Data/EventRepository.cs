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

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.LumaEvents.AsNoTracking();
        }

        public IEnumerable<Event> GetNextEvents()
        {
            return _context.LumaEvents.AsNoTracking().Where(x => x.EventUtcDateStart > DateTime.UtcNow);
        }

        public Event GetEventById(string eventId)
        {
            return _context.LumaEvents.AsNoTracking().FirstOrDefault(x => x.EventId == eventId);
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

        public Event GetEventByDateAndUser(Event userEvent)
        {
            return _context.LumaEvents.AsNoTracking().FirstOrDefault(x => 
                (x.EventUtcDateStart <= userEvent.EventUtcDateStart && x.EventUtcDateEnd >= userEvent.EventUtcDateStart) ||
                (x.EventUtcDateStart <= userEvent.EventUtcDateEnd && x.EventUtcDateEnd >= userEvent.EventUtcDateEnd));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
