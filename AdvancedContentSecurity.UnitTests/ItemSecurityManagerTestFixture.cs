using System;
using AdvancedContentSecurity.Core.ItemSecurity;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace AdvancedContentSecurity.UnitTests
{
    [TestFixture]
    public class ItemSecurityManagerTestFixture
    {
        #region [ Has Permission Tests ]

        [Test]
        public void HasPermission_correctly_set_returns_true()
        {
            // Arrange
            const string permissionName = "testpermission";
            ItemSecurityManagerTestHarness testHarness = new ItemSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            User user = AuthenticationManager.BuildVirtualUser("username", false);
            testHarness.ItemSecurityRepository.HasPermission(permissionName, item, user).Returns(true);

            // Act
            var result = testHarness.ItemSecurityManager.HasPermission(permissionName, item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void HasPermission_correctly_set_returns_false()
        {
            // Arrange
            const string permissionName = "testpermission";
            ItemSecurityManagerTestHarness testHarness = new ItemSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            User user = AuthenticationManager.BuildVirtualUser("username", false);
            testHarness.ItemSecurityRepository.HasPermission(permissionName, item, user).Returns(true);

            // Act
            var result = testHarness.ItemSecurityManager.HasPermission(permissionName, item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void HasPermission_item_null_excepts()
        {
            // Arrange
            const string permissionName = "testpermission";
            ItemSecurityManagerTestHarness testHarness = new ItemSecurityManagerTestHarness();
            User user = AuthenticationManager.BuildVirtualUser("username", false);

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.ItemSecurityManager.HasPermission(permissionName, null, user));

            // Assert
        }

        [Test]
        public void HasPermission_repository_exception_returns_false()
        {
            // Arrange
            const string permissionName = "testpermission";
            ItemSecurityManagerTestHarness testHarness = new ItemSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            User user = AuthenticationManager.BuildVirtualUser("username", false);
            testHarness.ItemSecurityRepository.When(x => x.HasPermission(permissionName, item, user)).Throw<Exception>();

            // Act
            var result = testHarness.ItemSecurityManager.HasPermission(permissionName, item, user);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        public class ItemSecurityManagerTestHarness
        {
            public ItemSecurityManagerTestHarness()
            {
                ItemSecurityRepository = Substitute.For<IItemSecurityRepository>();
                ItemSecurityManager = new ItemSecurityManager(ItemSecurityRepository);
            }

            public IItemSecurityRepository ItemSecurityRepository { get; private set; }

            public IItemSecurityManager ItemSecurityManager { get; private set; }
        }
    }
}
