using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace AdvancedContentSecurity.Core.Rules.Actions
{
    public class RemoveCurrentUserToRole<T> : RuleAction<T> where T : RuleContext
    {
        public string Role { get; set; }

        public override void Apply(T ruleContext)
        {
            if (Sitecore.Security.Accounts.Role.Exists(Role))
            {
                return;
            }

            var role = Sitecore.Security.Accounts.Role.FromName(Role);

            if (Sitecore.Context.User.Roles.Contains(role))
            {
                Sitecore.Context.User.Roles.Remove(role);
            }
        }
    }
}
