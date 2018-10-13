using System.Net.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Get host from Referrer header
        /// </summary>
        public static string GetReferrerHost(this HttpRequestMessage req) => req?.Headers.Referrer?.Host;
        /// <summary>
        /// Get host from request URI
        /// </summary>
        public static string GetRequestHost(this HttpRequestMessage req) => req?.RequestUri.Host;
    }
}
