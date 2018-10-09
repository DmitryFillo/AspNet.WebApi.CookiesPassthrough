using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace AspNet.WebApi.CookiesPassthrough.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class CookieDescriptorTests
    {
        [Test]
        [AutoData]
        public void EqualityTest(CookieDescriptor cookieDescriptor)
        {
            // Arrange
            var cookieDescriptorNew = new CookieDescriptor(cookieDescriptor.Name, cookieDescriptor.Value);

            // Act

            // Assert
            cookieDescriptorNew.Should().Be(cookieDescriptor);
        }

        [Test]
        [TestCase("localhost", "localhost", true, true)]
        [TestCase("www.localhost", "localhost", false, true)]
        [TestCase("www.localhost.ru", ".localhost.ru", true, true)]
        [TestCase("example.org", ".example.org", false, true)]
        [TestCase("www.example.org", ".example.org", true, true)]
        [TestCase("localhost", "localhost", true, false)]
        [TestCase("www.localhost", "www.localhost", false, false)]
        [TestCase("www.localhost.ru", "www.localhost.ru", true, false)]
        [TestCase("example.org", "example.org", false, false)]
        [TestCase("www.example.org", "www.example.org", true, false)]
        public void ToHttpHeaderTest(string domain, string domainInHeader, bool isHttpOnly, bool forAllSubdomains)
        {
            // Arrange
            var cookieDescriptor = new CookieDescriptor("a", "b")
            {
                Expires = new DateTime(1992, 12, 23),
                HttpOnly = isHttpOnly
            };
            var httpOnlyPart = isHttpOnly ? "HttpOnly; " : "";

            // Act
            var result = cookieDescriptor.ToHttpHeader(domain, forAllSubdomains);

            // Assert
            result.Should().BeEquivalentTo($"a=b; expires=Wed, 23 Dec 1992 00:00:00 GMT; {httpOnlyPart}domain={domainInHeader}; path=/");
        }

        [Test]
        public void ToHttpHeaderEmptyDomainTest()
        {
            // Arrange
            var cookieDescriptor = new CookieDescriptor("a", "b")
            {
                Expires = new DateTime(1992, 12, 23),
            };

            // Act
            var result = cookieDescriptor.ToHttpHeader("");

            // Assert
            result.Should().BeEquivalentTo("a=b; expires=Wed, 23 Dec 1992 00:00:00 GMT; path=/");
        }

        [Test]
        [AutoData]
        public void ToHttpHeadersTest(CookieDescriptor[] cookieDescriptors)
        {
            // Arrange

            // Act
            var result = cookieDescriptors.ToHttpHeaders("test");

            // Assert
            result.Count().Should().Be(cookieDescriptors.Length);
        }
    }
}
