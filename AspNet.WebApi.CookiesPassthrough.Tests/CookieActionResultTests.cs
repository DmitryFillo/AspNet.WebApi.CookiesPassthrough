using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AspNet.WebApi.CookiesPassthrough.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal class CookieActionResultTests
    {
        [Test]
        [AutoMoqData]
        public async Task SetCookieHeaderFromDescriptors_WithoutAllSubdomainsEnabledAsync(
            [Frozen] Mock<IHttpActionResult> actionResult,
            [Frozen] List<CookieDescriptor> cookieDescriptors,
            CancellationToken token,
            HttpResponseMessage responseMessage,
            string domain
        )
        {
            // Arrange
            var duplicatesCookies = cookieDescriptors.ToList();
            duplicatesCookies.Add(cookieDescriptors[1]);

            var sut = new CookieActionResult(actionResult.Object, duplicatesCookies, domain);

            actionResult.Setup(m => m.ExecuteAsync(It.Is<CancellationToken>(t => t == token))).Returns(Task.FromResult(responseMessage));

            // Act
            await sut.ExecuteAsync(token);

            // Assert
            actionResult.VerifyAll();
            responseMessage.Headers.GetValues("Set-Cookie").Should().BeEquivalentTo(cookieDescriptors.ToHttpHeaders(domain));
        }

        [Test]
        [AutoMoqData]
        public async Task SetCookieHeaderFromDescriptors_WithAllSubdomainsEnabledAsync(
            [Frozen] Mock<IHttpActionResult> actionResult,
            [Frozen] List<CookieDescriptor> cookieDescriptors,
            CancellationToken token,
            HttpResponseMessage responseMessage,
            string domain
        )
        {
            // Arrange
            var duplicatesCookies = cookieDescriptors.ToList();
            duplicatesCookies.Add(cookieDescriptors[1]);
            var sut = new CookieActionResult(actionResult.Object, duplicatesCookies, domain);
            actionResult.Setup(m => m.ExecuteAsync(It.Is<CancellationToken>(t => t == token))).Returns(Task.FromResult(responseMessage));

            // Act
            await sut.EnableCookiesForAllSubdomains().ExecuteAsync(token);

            // Assert
            actionResult.VerifyAll();
            responseMessage.Headers.GetValues("Set-Cookie").Should().BeEquivalentTo(cookieDescriptors.ToHttpHeaders(domain, true));
        }
    }
}
