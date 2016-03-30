using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace AdvancedContentSecurity.Core.Items
{
    [ExcludeFromCodeCoverage] // Wraps Sitecore Functionality - Integration testable only
    public class ItemRepository : IItemRepository
    {
        public string GetFieldValue(Item item, string fieldName)
        {
            return item[fieldName];
        }

        public IEnumerable<Item> GetItemsFromMultilist(Item item, string fieldName)
        {
            if (String.IsNullOrEmpty(item[fieldName]))
            {
                return null;
            }

            MultilistField multilistField = new MultilistField(item.Fields[fieldName]);
            return multilistField.GetItems();
        }

        public Item GetItemFromContextDatabase(ID itemId)
        {
            return Sitecore.Context.Database.GetItem(itemId);
        }

        public Item GetItemFromContentDatabase(ID itemId)
        {
            return Sitecore.Context.ContentDatabase.GetItem(itemId);
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
