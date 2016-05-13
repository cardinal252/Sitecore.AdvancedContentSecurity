using System;
using System.Collections.Generic;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Rules;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.ContentSecurity
{
    public class ContentSecurityManager : IContentSecurityManager
    {
        public ContentSecurityManager(IItemSecurityManager itemSecurityManager, IRulesManager rulesManager, IItemRepository itemRepository)
        {
            ItemSecurityManager = itemSecurityManager;
            RulesManager = rulesManager;
            ItemRepository = itemRepository;
        }

        protected IItemSecurityManager ItemSecurityManager { get; private set; }

        public IRulesManager RulesManager { get; private set; }

        public IItemRepository ItemRepository { get; set; }

        public virtual bool IsRuleReadAccessAllowed(Item item, Account account)
        {
            const bool defaultValue = true;

            ValidateItemAndUser(item, account);

            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, account) || 
                String.IsNullOrEmpty(ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.ReadRules)))
            {
                return defaultValue;
            }

            IEnumerable<Item> rulesItems = ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.ReadRules);

            return EvaluateRules(item, rulesItems);
        }

        public bool IsRuleReadAccessAllowed(CustomItemBase item, Account account)
        {
            return IsRuleReadAccessAllowed(item.InnerItem, account);
        }

        public virtual bool IsRestricted(Item item, Account account)
        {
            const bool defaultValue = false;
            ValidateItemAndUser(item, account);

            User user = account as User;

            bool isAdminUser = user != null && user.IsAdministrator;

            if (isAdminUser)
            {
                return defaultValue;
            }

            if (ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, account))
            {
                return true;
            }

            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, account) ||
                String.IsNullOrEmpty(ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)))
            {
                return defaultValue;
            }

            IEnumerable<Item> rulesItems = ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules);

            return EvaluateRules(item, rulesItems);
        }

        public bool IsRestricted(CustomItemBase item, Account account)
        {
            return IsRestricted(item.InnerItem, account);
        }

        private bool EvaluateRules(Item item, IEnumerable<Item> rulesItems)
        {
            if (rulesItems == null)
            {
                return true;
            }

            foreach (Item rulesItem in rulesItems)
            {
                if (RulesManager.EvaluateRulesFromField<RuleContext>(ContentSecurityConstants.FieldNames.Rule, rulesItem, item))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private static void ValidateItemAndUser(Item item, Account account)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }
        }
    }
}
