using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;
using AdvancedContentSecurity.Core.UserSecurity;

namespace AdvancedContentSecurity.Core.Configuration
{
    public static class AdvancedContentSecurityConfiguration
    {
        static AdvancedContentSecurityConfiguration()
        {
            ConfigurationFactory = new ConfigurationFactory();
        }

        public static IConfigurationFactory ConfigurationFactory { get; set; }
    }
}
