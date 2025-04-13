using FClub.Backend.Common.Exceptions;
using Humanizer;
using System.Collections.Concurrent;
using System.Net;

namespace FClub.Backend.Common.Middleware
{
    public sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new();

        public ExceptionResponse Map(Exception exception)
        {
            if (exception is FClubException ex)
            {
                if (exception is NotFoundException)
                {
                    return new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.NotFound);
                }
                else if (exception is ServiceUnavailableException)
                {
                    return new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.InternalServerError);
                }
                else
                {
                    return new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.BadRequest);
                }
            }
            else
            {
                throw exception;
            }
        }

        private record Error(string Code, string Message);

        private record ErrorsResponse(params Error[] Errors);

        private static string GetErrorCode(object exception)
        {
            var type = exception.GetType();
            return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
        }
    }
}
