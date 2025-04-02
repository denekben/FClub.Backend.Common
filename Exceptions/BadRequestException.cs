namespace FClub.Backend.Common.Exceptions
{
    public class BadRequestException : FClubException
    {
        public BadRequestException() : base() { }

        public BadRequestException(string message) : base(message) { }

    }
}
