using System;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough.Example.Controllers
{
    public class CookiesController : ApiController
    {
        [HttpGet]
        [Route("examples/{id}")]
        public IHttpActionResult GetExampleById([FromUri] int id)
        {
            var cookieDescriptors = new[]
            {
                // NOTE: it's possible to set cookies like this with '='
                new CookieDescriptor("test-cookie", "a=test-cookie"),

                // NOTE: duplicates will be automatically excluded
                new CookieDescriptor("test-cookie", "a=test-cookie"),

                // NOTE: automatic decode and secure cookie
                new CookieDescriptor("test-cookie2", "a%3Dtest-cookie") { Secure = true },

                // NOTE: with Expires
                new CookieDescriptor("test-cookie-id", id.ToString()) { HttpOnly = true, Expires = new DateTime(1992, 1, 1)},
            };

            switch (id)
            {
                case 1:
                    // NOTE: cookies with domain from request host
                    return Ok()
                        .AddCookies(cookieDescriptors, Request.GetRequestHost());
                case 2:
                    // NOTE: cookies for all subdomains
                    return Ok()
                        .AddCookies(cookieDescriptors, "example.org")
                        .EnableCookiesForAllSubdomains();
                case 3:
                    // NOTE: same as 2
                    return Ok()
                        .AddCookiesForAllSubdomains(cookieDescriptors, "example.org");
                case 4:
                    // NOTE: cookies with domain from referrer host
                    return Ok()
                        .AddCookiesForAllSubdomains(cookieDescriptors, Request.GetReferrerHost());
                case 5:
                    // NOTE: enable for all subdomains feature respects localhost and will not use dot before domain name
                    return Ok()
                        .AddCookies(cookieDescriptors, "localhost")
                        .EnableCookiesForAllSubdomains();
                case 6:
                    // NOTE: adds same cookies for different domains and makes last cookies domain with dot at the start
                    return Ok()
                        .AddCookies(cookieDescriptors, "example.org")
                        .AddCookies(cookieDescriptors, "example.net")
                        .EnableCookiesForAllSubdomains();
                default:
                    return BadRequest();
            }
        }
    }
}
