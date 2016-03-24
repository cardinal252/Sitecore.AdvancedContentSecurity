using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace ContentSecurity.Core.ItemSecurity
{
    public interface IItemSecurityRepository
    {
        bool HasPermission(string permissionName, Item item, User user);
    }
}
