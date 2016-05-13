using System;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ItemSecurity
{
    public class ItemSecurityManager : IItemSecurityManager
    {
        public ItemSecurityManager(IItemSecurityRepository itemSecurityRepository)
        {
            ItemSecurityRepository = itemSecurityRepository;
        }

        protected IItemSecurityRepository ItemSecurityRepository { get; private set; }

        public virtual bool HasPermission(string permissionName, Item item, Account account)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                return ItemSecurityRepository.HasPermission(permissionName, item, account);
            }
            catch (Exception ex)
            {
                // todo: do something with this
                return false;
            }
        }
    }
}
