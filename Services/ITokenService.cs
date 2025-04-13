using System.Security.Claims;

namespace FClub.Backend.Common.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid id, string firstName, string secondName, string? patronymic, string email, string role, string? audience = null);
        string GenerateInternalAccessToken(string? audience = null);
        string GenerateRefreshToken();
        DateTime GenerateRefreshTokenExpiresDate(double? refreshTokenLifeTime = null);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
