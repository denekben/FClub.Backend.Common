using FClub.Backend.Common.Exceptions;
using FClub.Backend.Common.Messages;
using FClub.Backend.Common.Messages.Publisher;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FClub.Backend.Common.Events.Publisher
{
    public class RabbitMqPublisher : IMessageBusPublisher
    {
        private readonly RabbitMqConnectionSettings _connectionSettings;
        private readonly PublisherOptions _options;
        private readonly ILogger<RabbitMqPublisher> _logger;

        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqPublisher(
            RabbitMqConnectionSettings connectionSettings,
            PublisherOptions options,
            ILogger<RabbitMqPublisher> logger = null)
        {
            _connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _connectionSettings.HostName,
                    Port = _connectionSettings.Port,
                    UserName = _connectionSettings.UserName,
                    Password = _connectionSettings.Password,
                    VirtualHost = _connectionSettings.VirtualHost,
                };

                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(
                    exchange: _options.ExchangeName,
                    type: _options.ExchangeType,
                    durable: _options.Durable,
                    autoDelete: _options.AutoDelete,
                    arguments: _options.ExchangeArguments);

                _connection.ConnectionShutdownAsync += OnConnectionShutdown;

                _logger?.LogInformation("RabbitMQ publisher initialized. Exchange: {ExchangeName}", _options.ExchangeName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to initialize RabbitMQ publisher");
                throw new MessageBusException("Failed to initialize RabbitMQ publisher");
            }
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (message == null)
            {
                _logger?.LogError("Attempted to publish null message");
                throw new ArgumentNullException(nameof(message));
            }

            if (_channel == null || _channel.IsClosed)
            {
                _logger?.LogError("RabbitMQ channel is not initialized");
                throw new InvalidOperationException("RabbitMQ channel is not available. Call InitializeAsync first.");
            }

            try
            {
                var messageType = typeof(TMessage).FullName;
                var messageBody = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(messageBody);

                var properties = new BasicProperties()
                {
                    Persistent = _options.PersistentMessages,
                    Type = messageType,
                    Headers = _options.MessageHeaders
                };

                await _channel.BasicPublishAsync(
                    exchange: _options.ExchangeName,
                    routingKey: _options.RoutingKey,
                    mandatory: _options.Mandatory,
                    basicProperties: properties,
                    body: body);

                _logger?.LogDebug("Published message of type {MessageType}", messageType);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to publish message of type {MessageType}", typeof(TMessage).FullName);
                throw new MessageBusException("Failed to publish message");
            }
        }

        public void Dispose()
        {
            try
            {
                _channel?.CloseAsync();
                _connection?.CloseAsync();

                _logger?.LogInformation("RabbitMQ publisher disposed");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error disposing RabbitMQ publisher");
            }
        }

        private async Task OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");

            await Task.CompletedTask;
        }
    }
}
