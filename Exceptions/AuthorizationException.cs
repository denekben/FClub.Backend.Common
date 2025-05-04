namespace FClub.Backend.Common.Exceptions
{
    public class AuthorizationException : FClubException
    {
        public AuthorizationException() : base() { }

        public AuthorizationException(string message) : base(message) { }

    }
}
