using FClub.Backend.Common.Events.Publisher;
using FClub.Backend.Common.Events.Subscriber;
using FClub.Backend.Common.BrokerMessaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;

public class MessageBusSubscriber : BackgroundService
{
    private readonly RabbitMqConnectionSettings _connectionSettings;
    private readonly SubscriberOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageBusSubscriber> _logger;

    private IConnection _connection;
    private IChannel _channel;
    private string _queueName;

    public MessageBusSubscriber(
        RabbitMqConnectionSettings connectionSettings,
        SubscriberOptions options,
        IServiceProvider serviceProvider,
        ILogger<MessageBusSubscriber> logger)
    {
        _connectionSettings = connectionSettings;
        _options = options;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    private async Task InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory
        {
            HostName = _connectionSettings.HostName,
            Port = _connectionSettings.Port,
            VirtualHost = _connectionSettings.VirtualHost,
            UserName = _connectionSettings.UserName,
            Password = _connectionSettings.Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: _options.ExchangeType
        );
        _queueName = string.IsNullOrEmpty(_options.QueueName)
            ? (await _channel.QueueDeclareAsync()).QueueName
            : _options.QueueName;

        await _channel.QueueBindAsync(
            queue: _queueName,
            exchange: _options.ExchangeName,
            routingKey: _options.RoutingKey
        );

        _logger.LogInformation("Listening on RabbitMQ (Exchange: {Exchange}, Queue: {Queue})",
            _options.ExchangeName, _queueName);

        _connection.ConnectionShutdownAsync += OnConnectionShutdown;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeRabbitMQ();

        var messageTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IMessage))
                        && t.GetConstructor(Type.EmptyTypes) != null)
            .ToDictionary(t => t.FullName!, t => t);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var messageTypeName = ea.BasicProperties.Type ?? Encoding.UTF8.GetString(ea.Body.ToArray());

                if (!messageTypes.TryGetValue(messageTypeName, out var messageType))
                {
                    _logger.LogWarning("Unknown message type: {MessageType}", messageTypeName);
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = (IMessage)JsonSerializer.Deserialize(messageJson, messageType)!;

                var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
                var handlers = scope.ServiceProvider.GetServices(handlerType);

                var processingTasks = handlers.Select(handler =>
                {
                    var handleMethod = handlerType.GetMethod("HandleAsync")!;
                    return (Task)handleMethod.Invoke(handler, new object[] { message, stoppingToken })!;
                });

                await Task.WhenAll(processingTasks);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Message deserialization failed");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: _options.PrefetchCount, global: false);
        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: consumer
        );

        await Task.Delay(Timeout.Infinite, stoppingToken); // Ожидание остановки
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }

    private async Task OnConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");

        await Task.CompletedTask;
    }
}