using AdvancedContentSecurity.Core.Testing;
using FluentAssertions;
using NUnit.Framework;

namespace AdvancedContentSecurity.UnitTests.Testing
{
    [TestFixture]
    public class AnonymousRepositoryTestFixture
    {
        [Test]
        public void Execute()
        {
            // Arrange
            IAnonymousRepository anonymousRepository = new AnonymousRepository();
            string target = "target";

            // Act
            anonymousRepository.Execute(() => { target = "changed"; });

            // Assert
            target.Should().Be("changed");
        }

        [Test]
        public void GetValue()
        {
            // Arrange
            IAnonymousRepository anonymousRepository = new AnonymousRepository();

            // Act
            var result = anonymousRepository.GetValue(() => "changed");

            // Assert
            result.Should().Be("changed");
        }

        [Test]
        public void GetValue_with_input()
        {
            // Arrange
            IAnonymousRepository anonymousRepository = new AnonymousRepository();
            string target = "target";

            // Act
            var result = anonymousRepository.GetValue(x => x.ToUpper(), target);

            // Assert
            result.Should().Be("TARGET");
        }
    }
}
