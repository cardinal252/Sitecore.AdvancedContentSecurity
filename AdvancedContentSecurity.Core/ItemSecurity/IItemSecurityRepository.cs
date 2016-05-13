using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ItemSecurity
{
    public interface IItemSecurityRepository
    {
        bool HasPermission(string permissionName, Item item, Account account);
    }
}
