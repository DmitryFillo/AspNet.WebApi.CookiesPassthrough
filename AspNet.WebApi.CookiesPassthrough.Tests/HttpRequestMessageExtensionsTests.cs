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
        public void GetClientDomainTest(HttpRequestMessage req)
        {
            // Arrange
            req.Headers.Referrer = new Uri("https://example.org/");

            // Act
            var result = req.GetClientHost();

            // Assert
            result.Should().Be("example.org");
        }

        [Test]
        [AutoMoqData]
        public void GetClientDomainNoReferrerTest(HttpRequestMessage req)
        {
            // Arrange

            // Act
            var result = req.GetClientHost();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void GetClientDomainNullTest()
        {
            // Arrange
            HttpRequestMessage req = null;

            // Act
            var result = req.GetClientHost();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
