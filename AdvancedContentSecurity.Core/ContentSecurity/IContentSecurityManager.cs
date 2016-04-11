using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ContentSecurity
{
    public interface IContentSecurityManager
    {
        bool IsRuleReadAccessAllowed(Item item, User user);

        bool IsRuleReadAccessAllowed(CustomItemBase item, User user);

        bool IsRestricted(Item item, User user);

        bool IsRestricted(CustomItemBase item, User user);
    }
}
