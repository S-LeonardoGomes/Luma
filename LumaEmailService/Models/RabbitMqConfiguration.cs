namespace LumaEmailService.Models
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; }
        public string Queue { get; set; }
        public int Port { get; internal set; }
    }
}
