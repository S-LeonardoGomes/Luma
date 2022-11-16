using RabbitMQ.Client;
using System.Text;

namespace LumaEventService.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqConfiguration:Host"]
            }.CreateConnection();

            _channel = _connection.CreateModel();
        }

        public void PublishMessage(string message, string queueName)
        {
            using (_connection)
            {
                using (_channel)
                {
                    _channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    byte[] bytesMessage = Encoding.UTF8.GetBytes(message);

                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        basicProperties: null,
                        body: bytesMessage
                    );
                }
            }
        }
    }
}
