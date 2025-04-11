using FClub.Backend.Common.InMemoryBrokerMessaging.Events;

namespace FClub.Backend.Common.InMemoryBrokerMessaging.Messaging
{
    internal interface IAsyncEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent;
    }
}
