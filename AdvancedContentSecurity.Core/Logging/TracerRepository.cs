using System.Diagnostics.CodeAnalysis;
using Sitecore.Diagnostics;

namespace AdvancedContentSecurity.Core.Logging
{
    [ExcludeFromCodeCoverage] // Wrapping static methods controlled by sitecore
    public class TracerRepository : ITracerRepository
    {
        public void Info(string message)
        {
            Tracer.Info(message);
        }

        public void Warning(string message, string details)
        {
            Tracer.Warning(message, details);
        }
    }
}
