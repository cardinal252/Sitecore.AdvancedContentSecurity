using System;
using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Rules.Conditions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace AdvancedContentSecurity.UnitTests.Rules.Conditions
{
    [ExcludeFromCodeCoverage] // Unit test fixture
    [TestFixture]
    public class IsRestrictedConditionTestFixture
    {
        [Test]
        public void Evaluate_when_restricted_returns_true()
        {
            // Arrange
            IsRestrictedConditionTestHarness testHarness = new IsRestrictedConditionTestHarness();
            RuleStack ruleStack = new RuleStack();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ContentSecurityManager.IsRestricted(item, user).Returns(true);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);

            RuleContext ruleContext = new RuleContext {Item = item };

            // Act
            testHarness.IsRestrictedCondition.Evaluate(ruleContext, ruleStack);

            // Assert
            ((bool) ruleStack.Pop()).Should().BeTrue();
        }

        [Test]
        public void Evaluate_when_not_restricted_returns_false()
        {
            // Arrange
            IsRestrictedConditionTestHarness testHarness = new IsRestrictedConditionTestHarness();
            RuleStack ruleStack = new RuleStack();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ContentSecurityManager.IsRestricted(item, user).Returns(false);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);

            RuleContext ruleContext = new RuleContext { Item = item };

            // Act
            testHarness.IsRestrictedCondition.Evaluate(ruleContext, ruleStack);

            // Assert
            ((bool)ruleStack.Pop()).Should().BeFalse();
        }

        [Test]
        public void Evaluate_when_null_user_gives_returns_false()
        {
            // Arrange
            IsRestrictedConditionTestHarness testHarness = new IsRestrictedConditionTestHarness();
            RuleStack ruleStack = new RuleStack();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            testHarness.ContentSecurityManager.When(x => x.IsRestricted(item, null)).Throw<ArgumentNullException>();

            RuleContext ruleContext = new RuleContext { Item = item };

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.IsRestrictedCondition.Evaluate(ruleContext, ruleStack));

            // Assert
        }

        [Test]
        public void Evaluate_when_null_item_gives_returns_false()
        {
            // Arrange
            IsRestrictedConditionTestHarness testHarness = new IsRestrictedConditionTestHarness();
            RuleStack ruleStack = new RuleStack();

            User user = AuthenticationManager.BuildVirtualUser("username", false);

            testHarness.ContentSecurityManager.When(x => x.IsRestricted(null, user)).Throw<ArgumentNullException>();

            RuleContext ruleContext = new RuleContext { Item = null };
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.IsRestrictedCondition.Evaluate(ruleContext, ruleStack));

            // Assert
        }

        public class IsRestrictedConditionTestHarness
        {
            public IsRestrictedConditionTestHarness()
            {
                ContentSecurityManager = Substitute.For<IContentSecurityManager>();
                SitecoreContextWrapper = Substitute.For<ISitecoreContextWrapper>();
                IsRestrictedCondition = new IsRestrictedCondition<RuleContext>(ContentSecurityManager, SitecoreContextWrapper);
            }

            public ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

            public IContentSecurityManager ContentSecurityManager { get; private set; }

            public IsRestrictedCondition<RuleContext> IsRestrictedCondition { get; private set; } 
        }
    }
}
