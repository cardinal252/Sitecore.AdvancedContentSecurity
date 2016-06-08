using System.Web;
using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace AdvancedContentSecurity.Core.Media
{
    public class MediaRequestHandler : Sitecore.Resources.Media.MediaRequestHandler
    {
        public MediaRequestHandler() : this(ConfigurationFactory.Default.GetContentSecurityManager(), ConfigurationFactory.Default.GetSitecoreContextWrapper())
        {
            
        }

        public MediaRequestHandler(IContentSecurityManager contentSecurityManager, ISitecoreContextWrapper sitecoreContextWrapper)
        {
            ContentSecurityManager = contentSecurityManager;
            SitecoreContextWrapper = sitecoreContextWrapper;
        }

        public IContentSecurityManager ContentSecurityManager { get; private set; }

        public ISitecoreContextWrapper SitecoreContextWrapper { get; set; }

        protected override bool DoProcessRequest(HttpContext context, MediaRequest request, Sitecore.Resources.Media.Media media)
        {
            Assert.ArgumentNotNull(context, "context");
            Assert.ArgumentNotNull(request, "request");
            Assert.ArgumentNotNull(media, "media");
            if(ContentSecurityManager.IsRestricted(media.MediaData.MediaItem.InnerItem, SitecoreContextWrapper.GetCurrentUser()))
            {
                PerformRestrictedAction(context, request, media);
                return false;
            }

            return base.DoProcessRequest(context, request, media);
        }

        protected virtual void PerformRestrictedAction(HttpContext context, MediaRequest request, Sitecore.Resources.Media.Media media)
        {
            context.Response.StatusCode = 403;
            context.Response.Status = "403 Forbidden";
            context.Response.End();
        }
    }
}
