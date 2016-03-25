using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;

namespace AdvancedContentSecurity.Core.Configuration
{
    public interface IConfigurationFactory
    {
        ITracerRepository TracerRepository { get; set; }

        IContentSecurityManager GetContentSecurityManager();

        IItemSecurityManager GetItemSecurityManager();

        IRulesManager GetRulesManager();
    }

}
