using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;

namespace AdvancedContentSecurity.Core.Configuration
{
    public static class AdvancedContentSecurityConfiguration
    {
        static AdvancedContentSecurityConfiguration()
        {
            ConfigurationFactory = new ConfigurationFactory(
                () => new ItemSecurityManager(new ItemSecurityRepository()), 
                () => new RulesManager(new RulesRepository()), 
                () => new ItemManager(), 
                x => new ContentSecurityManager(x.GetItemSecurityManager(), x.GetRulesManager(), x.GetItemManager()))
            {
                TracerRepository = new TracerRepository()
            };
        }

        public static IConfigurationFactory ConfigurationFactory { get; set; }
    }
}
