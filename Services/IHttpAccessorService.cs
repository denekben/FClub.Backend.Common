namespace FClub.Backend.Common.Services
{
    public interface IHttpContextService
    {
        public Guid GetCurrentUserId();
    }
}
