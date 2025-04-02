namespace FClub.Backend.Common.Exceptions
{
    public class MessageBusException : FClubException
    {
        public MessageBusException(string message)
            : base(message)
        {
        }
    }
}
