namespace FClub.Backend.Common.RabbitMQMessaging.Publisher
{
    public class PublisherOptions
    {
        public string ExchangeName { get; set; } = "messages";
        public string ExchangeType { get; set; } = "fanout";
        public string RoutingKey { get; set; } = "";
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public bool PersistentMessages { get; set; } = true;
        public bool Mandatory { get; set; } = false;
        public IDictionary<string, object> ExchangeArguments { get; set; } = new Dictionary<string, object>();
        public IDictionary<string, object> MessageHeaders { get; set; } = new Dictionary<string, object>();
    }
}
