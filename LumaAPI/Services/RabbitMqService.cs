using RabbitMQ.Client;
using System.Text;

namespace LumaEventService.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConfiguration _configuration;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;  
        }

        public void PublishMessage(string message, string queueName)
        {
            using (var _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqConfiguration:Host"],
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = "luma",
                Password = "RabbitMQ2022",
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            }.CreateConnection())
            {
                using (var _channel = _connection.CreateModel())
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
