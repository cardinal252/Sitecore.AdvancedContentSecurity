using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace AdvancedContentSecurity.Core.Rules.Conditions
{
    public class IsRestrictedCondition<T> : WhenCondition<T> where T : RuleContext
    {
        protected IContentSecurityManager ContentSecurityManager { get; private set; }

        protected ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        [ExcludeFromCodeCoverage] // Parameterless constructor
        public IsRestrictedCondition() : this(AdvancedContentSecurityConfiguration.ConfigurationFactory.GetContentSecurityManager(), new SitecoreContextWrapper())
        {
            
        }

        public IsRestrictedCondition(IContentSecurityManager contentSecurityManager, ISitecoreContextWrapper sitecoreContextWrapper)
        {
            ContentSecurityManager = contentSecurityManager;
            SitecoreContextWrapper = sitecoreContextWrapper;
        }

        protected override bool Execute(T ruleContext)
        {
            return ContentSecurityManager.IsRestricted(ruleContext.Item, SitecoreContextWrapper.GetCurrentUser());
        }
    }
}
