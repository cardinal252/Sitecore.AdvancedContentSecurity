using System;
using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace AdvancedContentSecurity.Core.Items
{
    public class ItemManager : IItemManager
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
    }
}
