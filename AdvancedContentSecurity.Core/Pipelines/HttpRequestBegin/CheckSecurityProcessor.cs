using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using Sitecore.Pipelines.HttpRequest;

namespace AdvancedContentSecurity.Core.Pipelines.HttpRequestBegin
{
    public class CheckSecurityProcessor : HttpRequestProcessor
    {
        [ExcludeFromCodeCoverage] // Parameterless constructor
        public CheckSecurityProcessor() : this (ConfigurationFactory.Default)
        {
        }

        [ExcludeFromCodeCoverage] // Chained constructor
        public CheckSecurityProcessor(IConfigurationFactory configurationFactory) : this(configurationFactory.GetContentSecurityManager(), configurationFactory.GetSitecoreContextWrapper())
        {
            
        }

        public CheckSecurityProcessor(IContentSecurityManager contentSecurityManager, ISitecoreContextWrapper sitecoreContextWrapper)
        {
            ContentSecurityManager = contentSecurityManager;
            SitecoreContextWrapper = sitecoreContextWrapper;
        }

        public IContentSecurityManager ContentSecurityManager { get; private set; }

        public ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        public override void Process(HttpRequestArgs args)
        {
            if (args == null || args.PermissionDenied || args.ProcessorItem == null || !Sitecore.Context.PageMode.IsNormal)
            {
                return;
            }

            args.PermissionDenied =
                !ContentSecurityManager.IsRuleReadAccessAllowed(SitecoreContextWrapper.GetContextItem(), SitecoreContextWrapper.GetCurrentUser());

            if (!args.PermissionDenied)
            {
                return;
            }

            SitecoreContextWrapper.SetContextItem(null);
        }
    }
}
