namespace FClub.Backend.Common.Exceptions
{
    public class ServiceUnavailableException : FClubException
    {
        public ServiceUnavailableException() : base() { }

        public ServiceUnavailableException(string message) : base(message) { }
    }
}
