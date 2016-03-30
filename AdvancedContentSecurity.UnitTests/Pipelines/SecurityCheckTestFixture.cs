using System;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Testing;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using SecurityCheck = AdvancedContentSecurity.Core.Pipelines.RenderLayout.SecurityCheck;

namespace AdvancedContentSecurity.UnitTests.Pipelines
{
    [TestFixture]
    public class SecurityCheckTestFixture
    {
        [Test]
        public void CanAccess_when_no_read_access_allowed_returns_true()
        {
            // Arrange
            var testHarness = new SecurityCheckTestHarness();
            testHarness.AnonymousRepository.GetValue(Arg.Any<Func<bool>>()).Returns(false);

            // Act
            var result = testHarness.SecurityCheck.CanAccess();

            // Assert
            result.Should().BeFalse();
            testHarness.TracerRepository.Received(1).Info(Arg.Any<string>());
        }

        [Test]
        public void CanAccess_when_read_access_allowed_and_no_context_item_returns_true()
        {
            // Arrange
            var testHarness = new SecurityCheckTestHarness();
            testHarness.AnonymousRepository.GetValue(Arg.Any<Func<bool>>()).Returns(true);

            // Act
            var result = testHarness.SecurityCheck.CanAccess();

            // Assert
            result.Should().BeTrue();
            testHarness.TracerRepository.Received(1).Info(Arg.Any<string>());
        }

        [Test]
        public void CanAccess_when_rule_read_access_is_allowed_returns_true()
        {
            // Arrange
            var testHarness = new SecurityCheckTestHarness();
            testHarness.AnonymousRepository.GetValue(Arg.Any<Func<bool>>()).Returns(true);

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";
            User user = AuthenticationManager.BuildVirtualUser("username", false);

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);
            testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, user).Returns(true);

            // Act
            var result = testHarness.SecurityCheck.CanAccess();

            // Assert
            result.Should().BeTrue();
            testHarness.TracerRepository.Received(1).Info(Arg.Any<string>());
        }

        [Test]
        public void CanAccess_when_rule_read_access_is_allowed_returns_false()
        {
            // Arrange
            var testHarness = new SecurityCheckTestHarness();
            testHarness.AnonymousRepository.GetValue(Arg.Any<Func<bool>>()).Returns(true);

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";
            User user = AuthenticationManager.BuildVirtualUser("username", false);

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            testHarness.SitecoreContextWrapper.GetContextItem().Returns(item);
            testHarness.SitecoreContextWrapper.GetCurrentUser().Returns(user);
            testHarness.ContentSecurityManager.IsRuleReadAccessAllowed(item, user).Returns(false);

            // Act
            var result = testHarness.SecurityCheck.CanAccess();

            // Assert
            result.Should().BeFalse();
            testHarness.TracerRepository.Received(1).Info(Arg.Any<string>());
        }

        public class SecurityCheckTestHarness
        {
            public SecurityCheckTestHarness()
            {
                SitecoreContextWrapper = Substitute.For<ISitecoreContextWrapper>();
                ContentSecurityManager = Substitute.For<IContentSecurityManager>();
                TracerRepository = Substitute.For<ITracerRepository>();
                AnonymousRepository = Substitute.For<IAnonymousRepository>();
                SecurityCheck = new SecurityCheck(SitecoreContextWrapper, ContentSecurityManager, TracerRepository,
                    AnonymousRepository);
            }

            public IContentSecurityManager ContentSecurityManager { get; private set; }

            public IAnonymousRepository AnonymousRepository { get; private set; }

            public ITracerRepository TracerRepository { get; private set; }

            public ISitecoreContextWrapper SitecoreContextWrapper { get; private set; }

            public SecurityCheck SecurityCheck { get; private set; }
        }
    }
}
