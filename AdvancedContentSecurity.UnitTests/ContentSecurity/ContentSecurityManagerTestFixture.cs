﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Rules;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace AdvancedContentSecurity.UnitTests.ContentSecurity
{
    [ExcludeFromCodeCoverage] // Unit test fixture
    [TestFixture]
    public class ContentSecurityManagerTestFixture
    {
        #region [ Security Restricted Tests ]

        [Test]
        public void IsRestricted_when_security_restricted_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsRestricted_when_administrator_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            User user = AuthenticationManager.BuildVirtualUser("username", false);
            user.RuntimeSettings.IsAdministrator = true;

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsRestricted_when_security_restricted_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsRestricted_for_role_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Role role = new TestRole("me\\test");

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, role)
                .Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, role);

            // Assert
            result.Should().BeTrue();
        }


        [Test]
        public void IsRestricted_for_role_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Role role = new TestRole("me\\test");

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, role)
                .Returns(false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, role);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region [ Rules based restriction tests ]

        [Test]
        public void IsRestricted_when_restricted_rule_evaluates_true_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            testHarness.RulesManager.EvaluateRulesFromField<RuleContext>(
                ContentSecurityConstants.FieldNames.Rule, rulesItem, item).Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsRestricted_when_user_is_administrator_and_restricted_rule_evaluates_true_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);
            user.RuntimeSettings.IsAdministrator = true;

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            testHarness.RulesManager.EvaluateRulesFromField<RuleContext>(
                ContentSecurityConstants.FieldNames.Rule, rulesItem, item).Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsRestricted_when_restricted_rule_evaluates_false_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            testHarness.RulesManager.EvaluateRulesFromField<RuleContext>(
                ContentSecurityConstants.FieldNames.Rule, rulesItem, item).Returns(false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsRestricted_when_not_rule_restricted_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeFalse();
        }

        public void IsRestricted_with_empty_rules_field_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)
                .Returns(null as string);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsRestricted_when_null_rules_list_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.RestrictedRules).Returns(null as IEnumerable<Item>);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.RestrictedRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, item, user)
                .Returns(false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRestricted(item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsRuleReadAccessAllowed_when_restricted_rule_evaluates_true_returns_false()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.ReadRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.ReadRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            testHarness.RulesManager.EvaluateRulesFromField<RuleContext>(
                ContentSecurityConstants.FieldNames.Rule, rulesItem, item).Returns(false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, user);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsRuleReadAccessAllowed_when_restricted_rule_evaluates_false_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.ReadRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.ReadRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            testHarness.RulesManager.EvaluateRulesFromField<RuleContext>(
                ContentSecurityConstants.FieldNames.Rule, rulesItem, item).Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsRuleReadAccessAllowed_with_empty_rules_field_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Guid rulesItemId = Guid.NewGuid();

            Item rulesItem = TestUtilities.GetTestItem(rulesItemId, templateId, branchId, itemName);
            List<Item> rulesItems = new List<Item> { rulesItem };
            testHarness.ItemRepository.GetItemsFromMultilist(item, ContentSecurityConstants.FieldNames.ReadRules).Returns(rulesItems);
            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.ReadRules)
                .Returns(null as string);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            // Act
            var result = testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, user);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsRuleReadAccessAllowed_null_rules_list_returns_true()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            testHarness.ItemRepository.GetFieldValue(item, ContentSecurityConstants.FieldNames.ReadRules)
                .Returns("qwerty");

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Rules, item, user)
                .Returns(true);

            // Act
            var result = testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, user);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region [ Argument Tests ]

        [Test]
        public void IsRestricted_with_null_item_excepts()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            User user = AuthenticationManager.BuildVirtualUser("username", false);
;

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.ContentSecurityManager.IsRestricted(null as Item, user));

            // Assert
        }

        [Test]
        public void IsRestricted_with_null_user_excepts()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            Assert.Throws<ArgumentNullException>(() =>  testHarness.ContentSecurityManager.IsRestricted(item, null));

            // Assert
        }

        [Test]
        public void IsRuleReadAccessAllowed_with_null_item_excepts()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            User user = AuthenticationManager.BuildVirtualUser("username", false);
            ;

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(null as Item, user));

            // Assert
        }

        [Test]
        public void IsRuleReadAccessAllowed_with_null_user_excepts()
        {
            // Arrange
            ContentSecurityManagerTestHarness testHarness = new ContentSecurityManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, null));

            // Assert
        }

        #endregion

        public class ContentSecurityManagerTestHarness
        {
            public ContentSecurityManagerTestHarness()
            {
                ItemSecurityManager = Substitute.For<IItemSecurityManager>();
                RulesManager = Substitute.For<IRulesManager>();
                ItemRepository = Substitute.For<IItemRepository>();
                ContentSecurityManager = new ContentSecurityManager(ItemSecurityManager, RulesManager, ItemRepository);
            }

            public IItemRepository ItemRepository { get; private set; }

            public IRulesManager RulesManager { get; private set; }

            public IItemSecurityManager ItemSecurityManager { get; private set; }

            public IContentSecurityManager ContentSecurityManager { get; private set; }
        }

        public class TestRole : Role
        {
            public TestRole(string roleName) : base(roleName)
            {
            }
        }
    }
}
