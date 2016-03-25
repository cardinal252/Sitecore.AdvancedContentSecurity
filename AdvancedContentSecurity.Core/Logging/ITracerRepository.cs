namespace AdvancedContentSecurity.Core.Logging
{
    public interface ITracerRepository
    {
        void Info(string message);

        void Warning(string message, string details);
    }
}
