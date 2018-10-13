﻿using System;
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
        public void Equality(CookieDescriptor cookieDescriptor)
        {
            // Arrange
            var cookieDescriptorNew = new CookieDescriptor(cookieDescriptor.Name, cookieDescriptor.Value);

            // Act

            // Assert
            cookieDescriptorNew.Should().Be(cookieDescriptor);
        }

        [Test]
        [TestCase("localhost", "localhost", true)]
        [TestCase("www.localhost", "localhost", true)]
        [TestCase("www.localhost.ru", ".localhost.ru", true)]
        [TestCase("example.org", ".example.org", true)]
        [TestCase("www.example.org", ".example.org", true)]
        [TestCase("localhost", "localhost", false)]
        [TestCase("www.localhost", "www.localhost", false)]
        [TestCase("www.localhost.ru", "www.localhost.ru", false)]
        [TestCase("example.org", "example.org", false)]
        [TestCase("www.example.org", "www.example.org", false)]
        public void ToHttpHeader_DifferentDomains(string domain, string domainInHeader, bool forAllSubdomains)
        {
            // Arrange
            var cookieDescriptor = new CookieDescriptor("a", "b");

            // Act
            var result = cookieDescriptor.ToHttpHeader(domain, forAllSubdomains);

            // Assert
            result.Should().BeEquivalentTo($"a=b; domain={domainInHeader}; path=/");
        }

        [Test]
        public void ToHttpHeader_DifferentFlags([Values]bool isHttpOnly, [Values]bool isSecure)
        {
            // Arrange
            const string domain = "www.example.org";

            var cookieDescriptor = new CookieDescriptor("a", "b")
            {
                Expires = new DateTime(1992, 12, 23),
                HttpOnly = isHttpOnly,
                Secure = isSecure,
            };
            var httpOnlyPart = isHttpOnly ? "HttpOnly; " : "";
            var securePart = isSecure ? "Secure; " : "";

            // Act
            var result = cookieDescriptor.ToHttpHeader(domain);

            // Assert
            result.Should().BeEquivalentTo($"a=b; expires=Wed, 23 Dec 1992 00:00:00 GMT; {httpOnlyPart}{securePart}domain={domain}; path=/");
        }

        [Test]
        public void ToHttpHeader_EmptyDomain()
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
        public void ToHttpHeader_IEnumerable(CookieDescriptor[] cookieDescriptors)
        {
            // Arrange

            // Act
            var result = cookieDescriptors.ToHttpHeaders("test");

            // Assert
            result.Count().Should().Be(cookieDescriptors.Length);
        }
    }
}
