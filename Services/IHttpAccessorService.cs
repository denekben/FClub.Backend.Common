namespace FClub.Backend.Common.Services
{
    public interface IHttpContextService
    {
        Guid? GetCurrentUserId();
        string? GetCurrentUserRoleName();
    }
}
