using System.Collections;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace AdvancedContentSecurity.Core.Items
{
    public interface IItemManager
    {
        string GetFieldValue(Item item, string fieldName);

        IEnumerable<Item> GetItemsFromMultilist(Item item, string fieldName);
    }
}
