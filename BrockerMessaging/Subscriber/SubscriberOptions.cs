namespace FClub.Backend.Common.Events.Subscriber
{
    public class SubscriberOptions
    {
        public string ExchangeName { get; set; } = "messages";
        public string ExchangeType { get; set; } = "fanout";
        public string QueueName { get; set; } = "";
        public string RoutingKey { get; set; } = "";
        public ushort PrefetchCount { get; set; } = 10;
    }
}
