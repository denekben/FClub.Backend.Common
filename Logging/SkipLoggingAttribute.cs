namespace FClub.Backend.Common.Logging
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SkipLoggingAttribute : Attribute
    {
    }
}
