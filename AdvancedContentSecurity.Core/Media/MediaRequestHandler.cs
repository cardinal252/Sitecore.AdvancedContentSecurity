using System.Web;
using AdvancedContentSecurity.Core.ContentSecurity;

namespace AdvancedContentSecurity.Core.Media
{
    public class MediaRequestHandler : Sitecore.Resources.Media.MediaRequestHandler
    {
        public MediaRequestHandler(IContentSecurityManager contentSecurityManager)
        {
            ContentSecurityManager = contentSecurityManager;
        }

        public IContentSecurityManager ContentSecurityManager { get; private set; }

        public override void ProcessRequest(HttpContext context)
        {
               
        }
    }
}
