using System.Diagnostics.CodeAnalysis;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.Context
{
    [ExcludeFromCodeCoverage] // Simple Wrapping Class for the Sitecore context
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

        public void SetContextItem(Item item)
        {
            Sitecore.Context.Item = item;
        }

        public string GetSiteName()
        {
            return Sitecore.Context.Domain.Name;
        }

        public bool HasContextDatabase()
        {
            return Sitecore.Context.Database != null;
        }

        public bool HasContentDatabase()
        {
            return Sitecore.Context.ContentDatabase != null;
        }

        public bool HasContextItem()
        {
            return Sitecore.Context.Item != null;
        }

        public void SetContextDevice(DeviceItem deviceItem)
        {
            Sitecore.Context.Device = deviceItem;
        }

        public void SetContextDevice(Item item)
        {
            DeviceItem deviceItem = new DeviceItem(item);
            SetContextDevice(deviceItem);
        }
    }
}
