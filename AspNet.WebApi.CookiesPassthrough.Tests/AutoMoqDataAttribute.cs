using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace AspNet.WebApi.CookiesPassthrough.Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        [ExcludeFromCodeCoverage]
        public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
