using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FClub.Backend.Common.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private const string _key = "JWT:SigningKey";
        private const string _lifeTime = "JWT:AccessTokenLifeTime";
        private const string _issuer = "JWT:Issuer";
        private const string _audience = "JWT:Audience";

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(Guid id, string lastname, string email, string role,
            string? key = null, string? lifeTime = null, string? issuer = null, string? audience = null)
        {
            if (!(key != null && lifeTime != null && issuer != null && audience != null) &&
                !(_config[_key] != null && _config[_lifeTime] != null && _config[_issuer] != null && _config[_audience] != null))
                throw new NullReferenceException("Invalid arguments for token service");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, lastname),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? _config[_key])),
                SecurityAlgorithms.HmacSha512Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(lifeTime ?? _config[_lifeTime])),
                SigningCredentials = creds,
                Issuer = issuer ?? _config[_issuer],
                Audience = audience ?? _config[_audience]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateAccessToken(string? key = null, string? lifeTime = null, string? issuer = null, string? audience = null)
        {
            if (!(key != null && lifeTime != null && issuer != null && audience != null) &&
                !(_config[_key] != null && _config[_lifeTime] != null && _config[_issuer] != null && _config[_audience] != null))
                throw new NullReferenceException("Invalid arguments for token service");

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? _config[_key])),
                SecurityAlgorithms.HmacSha512Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(lifeTime ?? _config[_lifeTime])),
                SigningCredentials = creds,
                Issuer = issuer ?? _config[_issuer],
                Audience = audience ?? _config[_audience]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string? key = null)
        {
            if (key == null && _config[_key] == null)
                throw new NullReferenceException("Invalid arguments for token service");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key ?? _config[_key])),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512))
                return null;
            return principal;
        }
    }
}
