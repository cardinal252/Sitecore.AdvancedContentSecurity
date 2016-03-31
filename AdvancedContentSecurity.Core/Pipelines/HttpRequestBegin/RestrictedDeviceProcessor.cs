using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.Logging;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace AdvancedContentSecurity.Core.Pipelines.HttpRequestBegin
{
    public class RestrictedDeviceProcessor : HttpRequestProcessor
    {
        [ExcludeFromCodeCoverage] // Allows parameterless construction
        public RestrictedDeviceProcessor()
            : this(ConfigurationFactory.Default)
        {
        }

        [ExcludeFromCodeCoverage] // Configuration factory chained constructor
        public RestrictedDeviceProcessor(IConfigurationFactory configurationFactory) : this(
            configurationFactory.GetSitecoreContextWrapper(), 
            configurationFactory.GetContentSecurityManager(), 
            configurationFactory.GetItemRepository(), 
            configurationFactory.TracerRepository)
        {
            
        }

        public RestrictedDeviceProcessor(ISitecoreContextWrapper sitecoreContextWrapper, IContentSecurityManager contentSecurityManager, IItemRepository itemRepository, ITracerRepository tracerRepository)
        {
            SitecoreContextWrapper = sitecoreContextWrapper;
            ContentSecurityManager = contentSecurityManager;
            ItemRepository = itemRepository;
            TracerRepository = tracerRepository;
        }

        protected ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        public IContentSecurityManager ContentSecurityManager { get; private set; }
        public IItemRepository ItemRepository { get; private set; }

        public ITracerRepository TracerRepository { get; private set; }

        [ExcludeFromCodeCoverage] // Delegates to testable method
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            SetDevice();
        }

        internal virtual void SetDevice()
        {
            if (SitecoreContextWrapper.GetContextItem() == null || SitecoreContextWrapper.GetSiteName().Equals("sitecore"))
            {
                return;
            }

            using (new ProfileSection("Resolve device."))
            {
                if (!SitecoreContextWrapper.HasContextDatabase())
                {
                    TracerRepository.Warning("No database in device resolver.", "Site is \"" + SitecoreContextWrapper.GetSiteName() + "\".");
                    return;
                }

                if (!ContentSecurityManager.IsRestricted(SitecoreContextWrapper.GetContextItem(), SitecoreContextWrapper.GetCurrentUser()))
                {
                    return;
                }

                var item = ItemRepository.GetItemFromContextDatabase(new ID(ContentSecurityConstants.Ids.Devices.RestrictedDeviceId));
                if (item != null)
                {
                    SitecoreContextWrapper.SetContextDevice(item);
                }
            }
        }
    }
}