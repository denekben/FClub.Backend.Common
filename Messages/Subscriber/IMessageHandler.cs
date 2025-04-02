using FClub.Backend.Common.Events.Publisher;

namespace FClub.Backend.Common.Events.Subscriber
{
    public interface IMessageHandler<in TEvent> where TEvent : class, IMessage
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
