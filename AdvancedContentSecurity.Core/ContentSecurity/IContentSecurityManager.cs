using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ContentSecurity
{
    public interface IContentSecurityManager
    {
        bool IsRuleReadAccessAllowed(Item item, Account account);

        bool IsRuleReadAccessAllowed(CustomItemBase item, Account account);

        bool IsRestricted(Item item, Account user);

        bool IsRestricted(CustomItemBase item, Account account);
    }
}
