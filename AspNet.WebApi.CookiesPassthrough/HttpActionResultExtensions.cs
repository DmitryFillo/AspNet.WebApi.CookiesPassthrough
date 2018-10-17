using System.Collections.Generic;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    /// <summary>
    /// <see cref="T:IHttpActionResult" /> extensions
    /// </summary>
    public static class HttpActionResultExtensions
    {
        /// <summary>
        /// Adds cookies to the <see cref="T:IHttpActionResult" />
        /// </summary>
        /// <param name="result"></param>
        /// <param name="cookieDescriptors"></param>
        /// <param name="domain">Domain for cookies</param>
        /// <returns></returns>
        public static CookieActionResult AddCookies(
            this IHttpActionResult result,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain) =>
            new CookieActionResult(result, cookieDescriptors, domain);

        /// <summary>
        /// Adds cookies to the <see cref="T:IHttpActionResult" /> and enables all of them for all subdomains
        /// </summary>
        /// <param name="result"></param>
        /// <param name="cookieDescriptors"></param>
        /// <param name="domain">Domain for cookies</param>
        /// <returns></returns>
        public static CookieActionResult AddCookiesForAllSubdomains(
            this IHttpActionResult result,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain) =>
            new CookieActionResult(result, cookieDescriptors, domain).EnableCookiesForAllSubdomains();
    }
}
