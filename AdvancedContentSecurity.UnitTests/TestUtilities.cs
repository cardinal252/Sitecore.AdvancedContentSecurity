using System;
using System.Runtime.Serialization;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace AdvancedContentSecurity.UnitTests
{
    public static class TestUtilities
    {
        public static Item GetTestItem(Guid itemId, Guid templateId, Guid branchId, string itemName)
        {
            ID actualItemId = new ID(itemId);
            Database database = FormatterServices.GetUninitializedObject(typeof(Database)) as Database;
            ItemDefinition itemDefinition = new ItemDefinition(actualItemId, itemName, new ID(templateId), new ID(branchId));
            ItemData itemData = new ItemData(itemDefinition, Language.Parse("en"), new Sitecore.Data.Version(1), new FieldList());
            Item item = new Item(actualItemId, itemData, database);
            return item;
        }
    }
}
