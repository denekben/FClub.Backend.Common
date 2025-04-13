using FClub.Backend.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FClub.Backend.Common.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenServiceOptions _options;

        public TokenService(TokenServiceOptions options)
        {
            _options = options;
        }

        public string GenerateAccessToken(Guid id, string firstName, string secondName, string? patronymic, string email, string role, string? audience = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, firstName + " " + secondName + ((" " + patronymic) ?? "")),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha512Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_options.AccessTokenLifeTime)),
                SigningCredentials = creds,
                Issuer = _options.Issuer,
                Audience = audience ?? _options.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateInternalAccessToken(string? audience = null)
        {
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha512Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = creds,
                Issuer = _options.Issuer,
                Audience = audience ?? _options.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            if (_options.Key == null)
                throw new NullReferenceException("Invalid arguments for token service");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_options.Key)),
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

        public DateTime GenerateRefreshTokenExpiresDate(double? refreshTokenLifeTime = null)
        {
            var lifetimeDays = refreshTokenLifeTime ?? Convert.ToDouble(_options.RefreshTokenLifeTime);

            if (lifetimeDays <= 0)
                throw new DomainException("Refresh token lifetime must be positive");

            return DateTime.UtcNow.AddDays(lifetimeDays);
        }
    }
}
