namespace FClub.Backend.Common.RabbitMQMessaging.Publisher
{
    public interface IMessageBusPublisher
    {
        Task InitializeAsync();
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
