using System.Security.Claims;

namespace FClub.Backend.Common.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid id, string firstName, string secondName, string? patronymic, string email, string role,
            string? key = null, string? lifeTime = null, string? issuer = null, string? audience = null);
        string GenerateAccessToken(string? key = null, string? lifeTime = null, string? issuer = null, string? audience = null);
        string GenerateRefreshToken();
        DateTime GenerateRefreshTokenExpiresDate(double? refreshTokenLifeTime = null);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string? key = null);
    }
}
