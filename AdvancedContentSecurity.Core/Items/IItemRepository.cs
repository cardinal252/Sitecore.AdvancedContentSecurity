using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace AdvancedContentSecurity.Core.Items
{
    public interface IItemRepository
    {
        string GetFieldValue(Item item, string fieldName);

        IEnumerable<Item> GetItemsFromMultilist(Item item, string fieldName);

        Item GetItemFromContextDatabase(ID itemId);

        Item GetItemFromContentDatabase(ID itemId);

        void SetContextDevice(Item item);

        void SetContextDevice(DeviceItem deviceItem);
    }
}
