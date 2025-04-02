namespace FClub.Backend.Common.Exceptions
{
    public abstract class FClubException : Exception
    {
        protected FClubException() : base()
        {
            Errors = new Dictionary<string, string[]>();
        }
        protected FClubException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }
        public IDictionary<string, string[]> Errors { get; protected set; }
    }
}
