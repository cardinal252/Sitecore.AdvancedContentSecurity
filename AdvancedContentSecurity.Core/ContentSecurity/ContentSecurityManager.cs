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

        public virtual bool IsRuleReadAccessAllowed(Item item, User user)
        {
            const bool defaultValue = true;

            ValidateItemAndUser(item, user);


            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user) || 
                String.IsNullOrEmpty(ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.ReadRules)))
            {
                return defaultValue;
            }

            IEnumerable<Item> rulesItems = ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.ReadRules);

            return EvaluateRules(item, rulesItems);
        }

        public bool IsRuleReadAccessAllowed(CustomItemBase item, User user)
        {
            return IsRuleReadAccessAllowed(item.InnerItem, user);
        }

        public virtual bool IsRestricted(Item item, User user)
        {
            const bool defaultValue = false;
            ValidateItemAndUser(item, user);

            if (ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user))
            {
                return true;
            }

            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user) ||
                String.IsNullOrEmpty(ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)))
            {
                return defaultValue;
            }

            IEnumerable<Item> rulesItems = ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules);

            return EvaluateRules(item, rulesItems);
        }

        public bool IsRestricted(CustomItemBase item, User user)
        {
            return IsRestricted(item.InnerItem, user);
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

        private static void ValidateItemAndUser(Item item, User user)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
        }
    }
}
