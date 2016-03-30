using System;
using AdvancedContentSecurity.Core;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Pipelines.HttpRequestBegin;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace AdvancedContentSecurity.UnitTests.Pipelines
{
    [TestFixture]
    public class RestrictedDeviceResolverTestFixture
    {
        public void SetDevice_with_no_context_does_nothing()
        {
            // Arrange
            var testHarness = new RestrictedDeviceResolverTestHarness();
            
            // Act
            testHarness.RestrictedDeviceProcessor.SetDevice();
            
            // Assert
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<Item>());
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<DeviceItem>());
        }

        [Test]
        public void SetDevice_with_sitecore_site_does_nothing()
        {
            // Arrange
            var testHarness = new RestrictedDeviceResolverTestHarness();
            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetSiteName().Returns("sitecore");


            // Act
            testHarness.RestrictedDeviceProcessor.SetDevice();

            // Assert
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<Item>());
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<DeviceItem>());
        }

        [Test]
        public void SetDevice_with_no_context_database_does_nothing()
        {
            // Arrange
            var testHarness = new RestrictedDeviceResolverTestHarness();
            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetSiteName().Returns("web");
            testHarness.SitecoreContextWrapper.HasContextDatabase().Returns(false);

            // Act
            testHarness.RestrictedDeviceProcessor.SetDevice();

            // Assert
            testHarness.TracerRepository.Received(1).Warning(Arg.Any<string>(), Arg.Any<string>());
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<Item>());
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<DeviceItem>());
        }

        [Test]
        public void SetDevice_on_item_which_is_not_restricted_does_nothing()
        {
            // Arrange
            var testHarness = new RestrictedDeviceResolverTestHarness();
            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            testHarness.SitecoreContextWrapper.GetSiteName().Returns("web");
            testHarness.SitecoreContextWrapper.HasContextDatabase().Returns(true);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);
            testHarness.ContentSecurityManager.IsRestricted(item, user).Returns(false);

            // Act
            testHarness.RestrictedDeviceProcessor.SetDevice();

            // Assert
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<Item>());
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<DeviceItem>());
        }

        [Test]
        public void SetDevice_on_item_which_is_restricted_sets_device()
        {
            // Arrange
            var testHarness = new RestrictedDeviceResolverTestHarness();
            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            testHarness.SitecoreContextWrapper.GetSiteName().Returns("web");
            testHarness.SitecoreContextWrapper.HasContextDatabase().Returns(true);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);
            testHarness.ContentSecurityManager.IsRestricted(item, user).Returns(true);

            Guid deviceItemId = Guid.NewGuid();
            Item deviceItem = TestUtilities.GetTestItem(deviceItemId, templateId, branchId, itemName);

            testHarness.ItemRepository.GetItemFromContextDatabase(new ID(ContentSecurityConstants.Ids.Devices.RestrictedDeviceId)).Returns(deviceItem);

            // Act
            testHarness.RestrictedDeviceProcessor.SetDevice();

            // Assert
            testHarness.SitecoreContextWrapper.Received(1).SetContextDevice(deviceItem);
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<DeviceItem>());
        }

        [Test]
        public void SetDevice_on_item_which_is_restricted_but_no_device_item_does_not_set_device()
        {
            // Arrange
            var testHarness = new RestrictedDeviceResolverTestHarness();
            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            testHarness.SitecoreContextWrapper.GetSiteName().Returns("web");
            testHarness.SitecoreContextWrapper.HasContextDatabase().Returns(true);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);
            testHarness.ContentSecurityManager.IsRestricted(item, user).Returns(true);

            testHarness.ItemRepository.GetItemFromContextDatabase(new ID(ContentSecurityConstants.Ids.Devices.RestrictedDeviceId)).Returns(null as Item);

            // Act
            testHarness.RestrictedDeviceProcessor.SetDevice();

            // Assert
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<Item>());
            testHarness.SitecoreContextWrapper.Received(0).SetContextDevice(Arg.Any<DeviceItem>());
        }

        public class RestrictedDeviceResolverTestHarness
        {
            public RestrictedDeviceResolverTestHarness()
            {
                SitecoreContextWrapper = Substitute.For<ISitecoreContextWrapper>();
                ContentSecurityManager = Substitute.For<IContentSecurityManager>();
                ItemRepository = Substitute.For<IItemRepository>();
                TracerRepository = Substitute.For<ITracerRepository>();
                RestrictedDeviceProcessor = new RestrictedDeviceProcessor(SitecoreContextWrapper, ContentSecurityManager, ItemRepository, TracerRepository);
            }

            public ITracerRepository TracerRepository { get; private set; }

            public IItemRepository ItemRepository { get; private set; }

            public IContentSecurityManager ContentSecurityManager { get; private set; }

            public ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

            public RestrictedDeviceProcessor RestrictedDeviceProcessor { get; private set; }
        }
    }
}
