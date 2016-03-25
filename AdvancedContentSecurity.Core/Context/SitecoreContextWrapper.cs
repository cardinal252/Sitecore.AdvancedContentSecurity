using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.Context
{
    public class SitecoreContextWrapper : ISitecoreContextWrapper
    {
        public User GetCurrentUser()
        {
            return Sitecore.Context.User;
        }

        public string GetCurrentUserName()
        {
            return Sitecore.Context.GetUserName();
        }

        public Item GetContextItem()
        {
            return Sitecore.Context.Item;
        }

        public string GetSiteName()
        {
            return Sitecore.Context.Domain.Name;
        }
    }
}
