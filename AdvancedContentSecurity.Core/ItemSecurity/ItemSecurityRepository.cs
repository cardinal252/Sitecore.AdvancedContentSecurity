using System.Diagnostics.CodeAnalysis;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ItemSecurity
{
    [ExcludeFromCodeCoverage] // Integration testable only
    public class ItemSecurityRepository : IItemSecurityRepository
    {
        public bool HasPermission(string permissionName, Item item, Account account)
        {
            AccessRight accessRight = new AccessRight(permissionName);
            return AuthorizationManager.IsAllowed(item, accessRight, account);
        }
    }
}
