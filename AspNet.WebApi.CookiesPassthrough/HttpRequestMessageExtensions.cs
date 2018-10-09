using System.Net.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetClientHost(this HttpRequestMessage req)
        {
            var referrer = req?.Headers.Referrer;
            return !string.IsNullOrWhiteSpace(referrer?.Host) ? referrer.Host : string.Empty;
        }
    }
}
