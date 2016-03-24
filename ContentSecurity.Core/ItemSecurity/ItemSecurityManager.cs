﻿using System;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace ContentSecurity.Core.ItemSecurity
{
    public class ItemSecurityManager : IItemSecurityManager
    {
        public ItemSecurityManager(IItemSecurityRepository itemSecurityRepository)
        {
            ItemSecurityRepository = itemSecurityRepository;
        }

        protected IItemSecurityRepository ItemSecurityRepository { get; private set; }

        public bool HasPermission(string permissionName, Item item, User user)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                return ItemSecurityRepository.HasPermission(permissionName, item, user);
            }
            catch (Exception ex)
            {
                // todo: do something with this
                return false;
            }
        }
    }
}