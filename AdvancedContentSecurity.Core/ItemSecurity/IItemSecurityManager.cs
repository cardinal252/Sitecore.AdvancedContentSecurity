using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ItemSecurity
{
    public interface IItemSecurityManager
    {
        bool HasPermission(string permissionName, Item item, User user);
    }
}
