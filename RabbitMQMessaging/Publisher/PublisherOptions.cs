namespace FClub.Backend.Common.RabbitMQMessaging.Publisher
{
    public class PublisherOptions
    {
        public string ExchangeName { get; set; } = "messages";
        public string ExchangeType { get; set; } = "fanout";
        public string RoutingKey { get; set; } = "";
        public bool Mandatory { get; set; } = false;
    }
}
