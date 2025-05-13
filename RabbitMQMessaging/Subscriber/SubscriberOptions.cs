using System.Reflection;

namespace FClub.Backend.Common.RabbitMQMessaging.Subscriber
{
    public class SubscriberOptions
    {
        public Assembly Assembly { get; set; }
        public string ExchangeName { get; set; } = "messages";
        public string ExchangeType { get; set; } = "fanout";
        public string RoutingKey { get; set; } = "";
    }
}
