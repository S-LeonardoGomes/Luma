using LumaEmailService.EmailService;
using LumaEmailService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LumaEmailService.RabbitMqService
{
    public class MessageConsumerService : BackgroundService
    {
        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public MessageConsumerService(IOptions<RabbitMqConfiguration> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _configuration = options.Value;
            ConnectionFactory factory = new()
            {
                HostName = _configuration.Host,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = "luma",
                Password = "RabbitMQ2022",
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                Ssl =
                {
                    ServerName = _configuration.Host,
                    Enabled = false
                }
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: _configuration.Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer consumer = new(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                byte[] contentArray = eventArgs.Body.ToArray();
                string contentString = Encoding.UTF8.GetString(contentArray);
                Email emailMessage = JsonSerializer.Deserialize<Email>(contentString);

                SendEmail(emailMessage);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.Queue, false, consumer);
            return Task.CompletedTask;
        }

        private void SendEmail(Email emailMessage)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                emailService.SendEmail(emailMessage);
            }
        }
    }
}
