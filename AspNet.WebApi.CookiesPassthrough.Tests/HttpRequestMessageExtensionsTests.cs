using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace AspNet.WebApi.CookiesPassthrough.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class HttpRequestMessageExtensionsTests
    {
        [Test]
        [AutoMoqData]
        public void GetReferrer_WithReferrer(HttpRequestMessage req)
        {
            // Arrange
            req.Headers.Referrer = new Uri("https://example.org/");

            // Act
            var result = req.GetReferrerHost();

            // Assert
            result.Should().Be("example.org");
        }

        [Test]
        [AutoMoqData]
        public void GetReferrer_NoReferrer(HttpRequestMessage req)
        {
            // Arrange

            // Act
            var result = req.GetReferrerHost();

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetReferrer_NullTest()
        {
            // Arrange
            HttpRequestMessage req = null;

            // Act
            var result = req.GetReferrerHost();

            // Assert
            result.Should().BeNull();
        }

        [Test]
        [AutoMoqData]
        public void GetHost(HttpRequestMessage req)
        {
            // Arrange
            req.RequestUri = new Uri("https://example.org/123");

            // Act
            var result = req.GetRequestHost();

            // Assert
            result.Should().Be("example.org");
        }

        [Test]
        public void GetHost_NullTest()
        {
            // Arrange
            HttpRequestMessage req = null;

            // Act
            var result = req.GetRequestHost();

            // Assert
            result.Should().BeNull();
        }
    }
}
