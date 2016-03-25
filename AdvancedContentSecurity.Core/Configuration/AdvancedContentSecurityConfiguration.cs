using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedContentSecurity.Core.ContentSecurity;
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
                x => new ContentSecurityManager(x.GetItemSecurityManager(), x.GetRulesManager()) )
            {
                TracerRepository = new TracerRepository()
            };
        }

        public static IConfigurationFactory ConfigurationFactory { get; set; }
    }
}
