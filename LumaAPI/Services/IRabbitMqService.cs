namespace LumaEventService.Services
{
    public interface IRabbitMqService
    {
        void PublishMessage(string message, string queueName);
    }
}
