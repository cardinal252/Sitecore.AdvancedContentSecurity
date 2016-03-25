using System;
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
        public ContentSecurityManager(IItemSecurityManager itemSecurityManager, IRulesManager rulesManager)
        {
            ItemSecurityManager = itemSecurityManager;
            RulesManager = rulesManager;
        }

        protected IItemSecurityManager ItemSecurityManager { get; private set; }

        public IRulesManager RulesManager { get; private set; }

        public virtual bool IsRuleReadAccessAllowed(Item item, User user)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user) || 
                String.IsNullOrEmpty(item[ContentSecurityConstants.FieldNames.ReadRules]) )
            {
                return true;
            }

            MultilistField rulesMultilistField = new MultilistField(item.Fields[ContentSecurityConstants.FieldNames.ReadRules]);

            foreach (Item rulesItem in rulesMultilistField.GetItems())
            {
                if (RulesManager.EvaluateRulesFromField<RuleContext>(ContentSecurityConstants.FieldNames.Rule, rulesItem, item))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public virtual bool IsRestricted(Item item, User user)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user))
            {
                return true;
            }

            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user) ||
                String.IsNullOrEmpty(item[ContentSecurityConstants.FieldNames.RestrictedRules]))
            {
                return false;
            }

            MultilistField rulesMultilistField = new MultilistField(item.Fields[ContentSecurityConstants.FieldNames.RestrictedRules]);

            foreach (Item rulesItem in rulesMultilistField.GetItems())
            {
                if (!RulesManager.EvaluateRulesFromField<RuleContext>(ContentSecurityConstants.FieldNames.Rule, rulesItem, item))
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
