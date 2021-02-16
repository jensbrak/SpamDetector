using FluentAssertions;
using System;
using Xunit;
using Zon3.SpamDetector;

namespace Zon3.SpamDetector.Tests
{
    public class SpamDetectorUnitTests
    {
        private SpamDetectorOptions _options;

        public SpamDetectorUnitTests()
        {
            _options = new SpamDetectorOptions();
        }

       [Fact]
        public void SpamDetectorOptionsShouldHaveValidIdentifier()
        {
            SpamDetectorOptions.Identifier.Should().BeEquivalentTo("SpamDetector");
        }

        [Fact]
        public void SpamDetectorOptionsShouldHaveSomeStringDefaults()
        {
            _options.SiteLanguage.Should().NotBeNullOrEmpty();
            _options.SiteEncoding.Should().NotBeNullOrEmpty();
            _options.UserRole.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void SpamDetectorOptionsShouldHaveIsTestSetToTrueByDefault()
        {
            _options.IsTest.Should().BeTrue();
        }
    }
}
