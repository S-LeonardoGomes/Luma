namespace LumaEventService.Models.DTO
{
    public class ReadEventDTO
    {
        public string EventId { get; private set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public DateTime EventLocalDateStart { get; set; }
        public DateTime EventLocalDateEnd { get; set; }
    }
}
