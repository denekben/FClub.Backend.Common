namespace FClub.Backend.Common.RabbitMQMessaging
{
    public class RabbitMqConnectionSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
    }
}
