using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;
using AdvancedContentSecurity.Core.UserSecurity;

namespace AdvancedContentSecurity.Core.Configuration
{
    public interface IConfigurationFactory
    {
        ITracerRepository TracerRepository { get; set; }

        IContentSecurityManager GetContentSecurityManager();

        IItemSecurityManager GetItemSecurityManager();

        IRulesManager GetRulesManager();

        IItemRepository GetItemRepository();

        IUserSecurityManager GetUserSecurityManager();

        ISitecoreContextWrapper GetSitecoreContextWrapper();
    }

}
