using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;

namespace AdvancedContentSecurity.Core.Pipelines.RenderLayout
{
    public class SecurityCheck : Sitecore.Pipelines.RenderLayout.SecurityCheck
    {
        protected ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        protected IContentSecurityManager ContentSecurityManager { get; private set; }
        public ITracerRepository TracerRepository { get; private set; }

        [ExcludeFromCodeCoverage] // Parameterless constructor
        public SecurityCheck() : this(
            new SitecoreContextWrapper(), 
            AdvancedContentSecurityConfiguration.ConfigurationFactory.GetContentSecurityManager(),
            AdvancedContentSecurityConfiguration.ConfigurationFactory.TracerRepository 
            )
        {

        }

        public SecurityCheck(ISitecoreContextWrapper sitecoreContextWrapper, IContentSecurityManager contentSecurityManager, ITracerRepository tracerRepository)
        {
            SitecoreContextWrapper = sitecoreContextWrapper;
            ContentSecurityManager = contentSecurityManager;
            TracerRepository = tracerRepository;
        }

        protected override bool HasAccess()
        {
            bool originalValue = base.HasAccess();
            if (!originalValue)
            {
                TracerRepository.Info("Access is denied as the current user \"" + SitecoreContextWrapper.GetCurrentUserName() + "\" has no read access to current item.");
                return false;
            }

            if (SitecoreContextWrapper.GetContextItem() != null)
            {
                return ContentSecurityManager.IsRuleReadAccessAllowed(SitecoreContextWrapper.GetContextItem(), SitecoreContextWrapper.GetCurrentUser());
            }

            TracerRepository.Info("Access is granted as there is no current item.");
            return true;
        }
    }
}
