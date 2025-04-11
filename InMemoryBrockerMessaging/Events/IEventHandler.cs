namespace FClub.Backend.Common.InMemoryBrokerMessaging.Events
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
