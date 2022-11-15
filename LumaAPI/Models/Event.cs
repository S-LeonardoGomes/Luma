namespace LumaEventService.Models
{
    public class Event
    {
        public string EventId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public DateTime EventUtcDateStart { get; set; }
        public DateTime EventUtcDateEnd { get; set; }
    }
}
