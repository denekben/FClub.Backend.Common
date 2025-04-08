using System.ComponentModel.DataAnnotations;

namespace FClub.Backend.Common.HttpMessaging
{
    public class HttpClientServiceOptions
    {
        [Required]
        public string HostName { get; set; }
        public string ServiceName { get; set; }
    }
}
