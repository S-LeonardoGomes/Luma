namespace LumaAuthenticationService.Services
{
    public interface IRabbitMqService
    {
        void PublishMessage(string message, string queueName);
    }
}
