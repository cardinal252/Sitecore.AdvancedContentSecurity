using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace AdvancedContentSecurity.Core.Context
{
    public interface ISitecoreContextWrapper
    {
        User GetCurrentUser();

        string GetCurrentUserName();

        Item GetContextItem();

        void SetContextItem(Item item);

        string GetSiteName();

        bool HasContextDatabase();

        bool HasContentDatabase();

        bool HasContextItem();

        void SetContextDevice(Item item);

        void SetContextDevice(DeviceItem deviceItem);
    }
}