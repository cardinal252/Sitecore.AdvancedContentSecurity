using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.Logging;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;

namespace AdvancedContentSecurity.Core.Pipelines.HttpRequestBegin
{
    public class RestrictedDeviceProcessor : HttpRequestProcessor
    {
        public RestrictedDeviceProcessor()
            : this(
                  new SitecoreContextWrapper(), 
                  AdvancedContentSecurityConfiguration.ConfigurationFactory.GetContentSecurityManager(),
                  AdvancedContentSecurityConfiguration.ConfigurationFactory.GetItemRepository(),
                  AdvancedContentSecurityConfiguration.ConfigurationFactory.TracerRepository)
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

        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (SitecoreContextWrapper.GetContextItem() == null || SitecoreContextWrapper.GetSiteName().Equals("sitecore"))
            {
                return;
            }

            using (new ProfileSection("Resolve device."))
            {
                if (SitecoreContextWrapper.HasContextDatabase())
                {
                    TracerRepository.Warning("No database in device resolver.", "Site is \"" + SitecoreContextWrapper.GetSiteName() + "\".");
                }
                else
                {
                    if (!ContentSecurityManager.IsRestricted(SitecoreContextWrapper.GetContextItem(), SitecoreContextWrapper.GetCurrentUser()))
                    {
                        return;
                    }

                    var item = ItemRepository.GetItemFromContextDatabase(new ID(ContentSecurityConstants.Ids.Devices.RestrictedDeviceId));
                    if (item != null)
                    {
                        ItemRepository.SetContextDevice(item);
                    }
                }
            }
        }
    }
}