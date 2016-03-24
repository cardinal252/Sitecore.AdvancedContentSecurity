using ContentSecurity.Core.ItemSecurity;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace ContentSecurity.Core.Rules
{
    public class IsRestrictedCondition<T> : WhenCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            // todo: do this better
            IItemSecurityManager itemSecurityManager = new ItemSecurityManager(new ItemSecurityRepository());
            return itemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, Sitecore.Context.Item, Sitecore.Context.User);
        }
    }
}
