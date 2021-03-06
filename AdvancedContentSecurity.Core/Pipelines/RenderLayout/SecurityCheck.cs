﻿using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Logging;

namespace AdvancedContentSecurity.Core.Pipelines.RenderLayout
{
    public class SecurityCheck : Sitecore.Pipelines.RenderLayout.SecurityCheck
    {
        [ExcludeFromCodeCoverage] // Parameterless constructor
        public SecurityCheck() : this(ConfigurationFactory.Default)
        {

        }

        [ExcludeFromCodeCoverage] // Configuration factory chained constructor
        public SecurityCheck(IConfigurationFactory configurationFactory) : this(new SitecoreContextWrapper(), configurationFactory.GetContentSecurityManager(), configurationFactory.TracerRepository)
        {
            
        }

        public SecurityCheck(ISitecoreContextWrapper sitecoreContextWrapper, IContentSecurityManager contentSecurityManager, ITracerRepository tracerRepository)
        {
            SitecoreContextWrapper = sitecoreContextWrapper;
            ContentSecurityManager = contentSecurityManager;
            TracerRepository = tracerRepository;
        }

        protected ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        protected IContentSecurityManager ContentSecurityManager { get; private set; }

        public ITracerRepository TracerRepository { get; private set; }

        [ExcludeFromCodeCoverage] // overrides sitecores
        protected override bool HasAccess()
        {
            return CanAccess(base.HasAccess());
        }

        internal virtual bool CanAccess(bool originalValue)
        {
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
