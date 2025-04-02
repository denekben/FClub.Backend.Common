using FClub.Backend.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FClub.Backend.Common.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new BadRequestException("Cannot find user");

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new BadRequestException("User ID is not a valid guid");
            }

            return userId;
        }

        public string GetCurrentUserRoleName()
        {
            var roleName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new BadRequestException("Cannot find user's role");

            return roleName;
        }
    }
}
