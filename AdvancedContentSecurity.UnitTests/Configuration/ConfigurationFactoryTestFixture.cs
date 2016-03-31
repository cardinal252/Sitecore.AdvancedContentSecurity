using AdvancedContentSecurity.Core.Configuration;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;
using AdvancedContentSecurity.Core.UserSecurity;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AdvancedContentSecurity.UnitTests.Configuration
{
    [TestFixture]
    public class ConfigurationFactoryTestFixture
    {
        [Test]
        public void ConfigurationTestFactory_setup()
        {
            // Arrange
            IItemSecurityManager itemSecurityManager = Substitute.For<IItemSecurityManager>();
            IRulesManager rulesManager = Substitute.For<IRulesManager>();
            IItemRepository itemRepository = Substitute.For<IItemRepository>();
            IContentSecurityManager contentSecurityManager = Substitute.For<IContentSecurityManager>();
            IUserSecurityManager userSecurityManager = Substitute.For<IUserSecurityManager>();
            ISitecoreContextWrapper sitecoreContextWrapper = Substitute.For<ISitecoreContextWrapper>();
            ITracerRepository tracerRepository = new TracerRepository();

            // Act

            IConfigurationFactory configurationFactory = new ConfigurationFactory(
                () => itemSecurityManager,
                () => rulesManager,
                () => itemRepository,
                x => contentSecurityManager,
                x => userSecurityManager,
                tracerRepository,
                () => sitecoreContextWrapper);

            // Assert
            configurationFactory.GetItemSecurityManager().Should().Be(itemSecurityManager);
            configurationFactory.GetRulesManager().Should().Be(rulesManager);
            configurationFactory.GetItemRepository().Should().Be(itemRepository);
            configurationFactory.GetContentSecurityManager().Should().Be(contentSecurityManager);
            configurationFactory.GetUserSecurityManager().Should().Be(userSecurityManager);
            configurationFactory.TracerRepository.Should().Be(tracerRepository);
            configurationFactory.GetSitecoreContextWrapper().Should().Be(sitecoreContextWrapper);
        }
    }
}
