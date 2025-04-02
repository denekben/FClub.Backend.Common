using System.Net;

namespace FClub.Backend.Common.Middleware
{
    public sealed record ExceptionResponse(object Response, HttpStatusCode StatusCode);
}
