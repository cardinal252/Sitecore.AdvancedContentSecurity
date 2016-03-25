using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
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
            : this(new SitecoreContextWrapper(), AdvancedContentSecurityConfiguration.ConfigurationFactory.GetContentSecurityManager(), AdvancedContentSecurityConfiguration.ConfigurationFactory.TracerRepository)
        {
        }

        public RestrictedDeviceProcessor(ISitecoreContextWrapper sitecoreContextWrapper, IContentSecurityManager contentSecurityManager, ITracerRepository tracerRepository)
        {
            SitecoreContextWrapper = sitecoreContextWrapper;
            ContentSecurityManager = contentSecurityManager;
            TracerRepository = tracerRepository;
        }

        protected ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

        public IContentSecurityManager ContentSecurityManager { get; private set; }

        public ITracerRepository TracerRepository { get; set; }

        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (SitecoreContextWrapper.GetContextItem() == null || SitecoreContextWrapper.GetSiteName().Equals("sitecore"))
            {
                return;
            }

            using (new ProfileSection("Resolve device."))
            {
                Database database = Sitecore.Context.Database;
                SiteContext site = Sitecore.Context.Site;
                string str = site != null ? site.Name : string.Empty;
                if (database == null)
                {
                    TracerRepository.Warning("No database in device resolver.", "Site is \"" + str + "\".");
                }
                else
                {
                    if (!ContentSecurityManager.IsRestricted(SitecoreContextWrapper.GetContextItem(), SitecoreContextWrapper.GetCurrentUser()))
                    {
                        return;
                    }

                    var item = database.GetItem(new ID(ContentSecurityConstants.Ids.Devices.RestrictedDeviceId));
                    DeviceItem deviceItem = new DeviceItem(item);
                    Sitecore.Context.Device = deviceItem;
                }
            }
        }
    }
}