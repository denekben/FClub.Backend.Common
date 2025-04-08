namespace FClub.Backend.Common.Events.Publisher
{
    public interface IMessageBusPublisher
    {
        Task InitializeAsync();
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
