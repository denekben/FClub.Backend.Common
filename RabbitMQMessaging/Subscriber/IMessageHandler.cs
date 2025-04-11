using FClub.Backend.Common.RabbitMQMessaging.Publisher;

namespace FClub.Backend.Common.RabbitMQMessaging.Subscriber
{
    public interface IMessageHandler<in TEvent> where TEvent : class, IMessage
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
