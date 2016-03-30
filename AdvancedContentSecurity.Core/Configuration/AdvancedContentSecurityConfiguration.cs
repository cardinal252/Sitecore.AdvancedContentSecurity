using System.Diagnostics.CodeAnalysis;

namespace AdvancedContentSecurity.Core.Configuration
{
    [ExcludeFromCodeCoverage] // Simple singleton implementation
    public static class AdvancedContentSecurityConfiguration
    {
        static AdvancedContentSecurityConfiguration()
        {
            ConfigurationFactory = new ConfigurationFactory();
        }

        public static IConfigurationFactory ConfigurationFactory { get; set; }
    }
}
