namespace EventService.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime UtcEventDateStart { get; set; }
        public DateTime UtcEventDateEnd { get; set; }
    }
}
