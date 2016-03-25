using AdvancedContentSecurity.Core.ItemSecurity;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace AdvancedContentSecurity.Core.Pipelines.RenderLayout
{
    public class SecurityCheck : Sitecore.Pipelines.RenderLayout.SecurityCheck
    {
        protected IItemSecurityManager ItemSecurityManager { get; private set; }

        public SecurityCheck() : this(new ItemSecurityManager(new ItemSecurityRepository()))
        {

        }

        public SecurityCheck(IItemSecurityManager itemSecurityManager)
        {
            ItemSecurityManager = itemSecurityManager;
        }

        protected override bool HasAccess()
        {
            bool originalValue = base.HasAccess();
            if (!originalValue)
            {
                Tracer.Info("Access is denied as the current user \"" + Context.GetUserName() + "\" has no read access to current item.");
                return false;
            }

            if (Context.Item == null)
            {
                Tracer.Info("Access is granted as there is no current item.");
                return true;
            }

            if (!ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.RulesRead, Context.Item, Context.User))
            {
                // true is the original value
                return true;
            }

            // todo: evaluate rules
            if (Context.Item.ID == new ID("{536FDC66-788F-4641-90BF-F05F1F8B5D4F}"))
            {
                return false;
            }
            
            // true is the original value
            return true;
        }
    }
}
