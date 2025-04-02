
namespace FClub.Backend.Common.Exceptions
{
    public class NotFoundException : FClubException
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
    }
}
