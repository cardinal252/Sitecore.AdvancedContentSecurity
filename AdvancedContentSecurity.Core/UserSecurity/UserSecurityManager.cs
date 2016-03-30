using System;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.Rules;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.UserSecurity
{
    public class UserSecurityManager : IUserSecurityManager
    {
        public UserSecurityManager(IRulesManager rulesManager, IItemRepository itemRepository)
        {
            RulesManager = rulesManager;
            ItemRepository = itemRepository;
        }

        protected IRulesManager RulesManager { get; private set; }

        protected IItemRepository ItemRepository { get; private set; }

        public void ApplySecurityFromRules(User user)
        {
            ApplySecurityFromRules(user, ItemRepository.GetItemFromContextDatabase(new ID(ContentSecurityConstants.Ids.Settings.ConfigurationItemId)));
        }

        public virtual void ApplySecurityFromRules(User user, Item configurationItem)
        {
            if (configurationItem == null)
            {
                throw new ArgumentNullException(nameof(configurationItem));
            }

            // todo : add logging
            RulesManager.ExecuteRulesFromField<RuleContext>(ContentSecurityConstants.FieldNames.UserInitialisationRule, configurationItem);
        }
    }
}
