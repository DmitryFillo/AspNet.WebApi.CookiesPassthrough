using System.Collections.Generic;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    public static class HttpActionResultExtensions
    {
        public static IHttpActionResult AddCookies(
            this IHttpActionResult result,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain,
            bool forAllSubdomains = true) =>
            new CookieActionResult(result, cookieDescriptors, domain, forAllSubdomains);
    }
}
