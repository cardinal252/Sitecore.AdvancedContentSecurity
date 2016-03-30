using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.UserSecurity
{
    public interface IUserSecurityManager
    {
        void ApplySecurityFromRules(User user);

        void ApplySecurityFromRules(User user, Item configurationItemId);
    }
}
