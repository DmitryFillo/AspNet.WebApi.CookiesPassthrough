using System.Collections.Generic;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    public static class HttpActionResultExtensions
    {
        public static CookieActionResult AddCookies(
            this IHttpActionResult result,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain) =>
            new CookieActionResult(result, cookieDescriptors, domain);

        public static CookieActionResult AddCookiesForAllSubdomains(
            this IHttpActionResult result,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain) =>
            new CookieActionResult(result, cookieDescriptors, domain).EnableCookiesForAllSubdomains();
    }
}
