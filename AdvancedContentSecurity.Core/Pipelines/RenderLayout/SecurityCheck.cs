using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Testing;

namespace AdvancedContentSecurity.Core.Pipelines.RenderLayout
{
    public class SecurityCheck : Sitecore.Pipelines.RenderLayout.SecurityCheck
    {
        [ExcludeFromCodeCoverage] // Parameterless constructor
        public SecurityCheck() : this(
            new SitecoreContextWrapper(), 
            AdvancedContentSecurityConfiguration.ConfigurationFactory.GetContentSecurityManager(),
            AdvancedContentSecurityConfiguration.ConfigurationFactory.TracerRepository,
            new AnonymousRepository())
        {

        }

        public SecurityCheck(ISitecoreContextWrapper sitecoreContextWrapper, IContentSecurityManager contentSecurityManager, ITracerRepository tracerRepository, IAnonymousRepository anonymousRepository)
        {
            SitecoreContextWrapper = sitecoreContextWrapper;
            ContentSecurityManager = contentSecurityManager;
            TracerRepository = tracerRepository;
            AnonymousRepository = anonymousRepository;
        }

        protected ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        protected IContentSecurityManager ContentSecurityManager { get; private set; }

        public ITracerRepository TracerRepository { get; private set; }

        public IAnonymousRepository AnonymousRepository { get; private set; }

        [ExcludeFromCodeCoverage] // overrides sitecores
        protected override bool HasAccess()
        {
            return CanAccess();
        }

        internal virtual bool CanAccess()
        {
            bool originalValue = AnonymousRepository.GetValue(() => base.HasAccess());
            if (!originalValue)
            {
                TracerRepository.Info("Access is denied as the current user \"" + SitecoreContextWrapper.GetCurrentUserName() + "\" has no read access to current item.");
                return false;
            }

            if (SitecoreContextWrapper.GetContextItem() != null)
            {
                TracerRepository.Info("Access is determined by the rule.");
                return ContentSecurityManager.IsRuleReadAccessAllowed(SitecoreContextWrapper.GetContextItem(), SitecoreContextWrapper.GetCurrentUser());
            }

            TracerRepository.Info("Access is granted as there is no current item.");
            return true;
        }
    }
}
