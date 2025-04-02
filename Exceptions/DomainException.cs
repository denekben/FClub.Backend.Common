namespace FClub.Backend.Common.Exceptions
{
    public class DomainException : FClubException
    {
        public DomainException() : base() { }
        public DomainException(string message) : base(message) { }
    }
}
