using LumaEventService.Models;
using Microsoft.EntityFrameworkCore;

namespace LumaEventService.Data.Context
{
    public class EventServiceContext : DbContext
    {
        public EventServiceContext(DbContextOptions<EventServiceContext> options) : base(options) { }

        public DbSet<Event> LumaEvents { get; set; }
    }
}
