namespace FClub.Backend.Common.RabbitMQMessaging
{
    public class RabbitMqConnectionSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}
