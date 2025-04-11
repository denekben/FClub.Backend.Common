using FClub.Backend.Common.InMemoryBrokerMessaging.Events;

namespace FClub.Backend.Common.InMemoryBrokerMessaging.Messaging
{
    public interface IMessageBroker
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
    }
}
