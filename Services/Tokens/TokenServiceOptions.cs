using System.ComponentModel.DataAnnotations;

namespace FClub.Backend.Common.Services
{
    public class TokenServiceOptions
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string AccessTokenLifeTime { get; set; }
        [Required]
        public string RefreshTokenLifeTime { get; set; }
        [Required]
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
