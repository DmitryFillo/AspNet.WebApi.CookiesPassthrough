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

                // NOTE: duplicates (both name and value) will be automatically excluded
                new CookieDescriptor("test-cookie", "a=test-cookie"),

                // NOTE: with space
                new CookieDescriptor("test-cookie1", "a=test cookie"),

                // NOTE: with space and encode
                new CookieDescriptor("test-cookie2", "a=test cookie") { CodeStatus = CookieCodeStatus.Encode },

                // NOTE: decode and secure cookie
                new CookieDescriptor("test-cookie3", "a%3Dtest-cookie") { Secure = true, CodeStatus = CookieCodeStatus.Decode },

                // NOTE: no encode or decode
                new CookieDescriptor("test-cookie4", "a%3Dtest-cookie =2"),

                // NOTE: with Expires
                new CookieDescriptor("test-cookie5", id.ToString()) { HttpOnly = true, Expires = new DateTime(1992, 1, 1)},

                // NOTE: with Expires
                new CookieDescriptor("test-cookie6", "xxx") { Path = "/subfolder/" },
            };

            switch (id)
            {
                case 1:
                    // NOTE: cookies with domain from request host
                    return Ok()
                        .AddCookies(cookieDescriptors, Request.GetRequestHost());
                case 2:
                    // NOTE: cookies for all subdomains, domain will be ".example.org"
                    return Ok()
                        .AddCookies(cookieDescriptors, "www.example.org")
                        .EnableCookiesForAllSubdomains();
                case 3:
                    // NOTE: same as 2, but w/o www
                    return Ok()
                        .AddCookiesForAllSubdomains(cookieDescriptors, "example.org");
                case 4:
                    // NOTE: cookies with domain from referrer host
                    return Ok()
                        .AddCookiesForAllSubdomains(cookieDescriptors, Request.GetReferrerHost());
                case 5:
                    // NOTE: domain will be empty to make localhost cookies work
                    return Ok()
                        .AddCookies(cookieDescriptors, "localhost")
                        .EnableCookiesForAllSubdomains();
                case 6:
                    // NOTE: adds same cookies for different domains and makes last cookies domain with dot at the start
                    return Ok()
                        .AddCookies(cookieDescriptors, "example.org")
                        .AddCookies(cookieDescriptors, "example.net")
                        .EnableCookiesForAllSubdomains();
                case 7:
                    // NOTE: same as 6, but no "www" exclude in last case, because domain already enabled for all subdomains
                    return Ok()
                        .AddCookies(cookieDescriptors, "example.org")
                        .AddCookies(cookieDescriptors, ".www.example.net")
                        .EnableCookiesForAllSubdomains();
                case 8:
                    // NOTE: same result as 7
                    return Ok()
                        .AddCookies(cookieDescriptors, "example.org")
                        .AddCookies(cookieDescriptors, ".www.example.net");
                case 9:
                    // NOTE: domain will be ".www.org", no "www" exclude
                    return Ok()
                        .AddCookiesForAllSubdomains(cookieDescriptors, "www.org");
                case 10:
                    // NOTE: domain will be empty to make localhost cookies work
                    return Ok()
                        .AddCookies(cookieDescriptors, ".localhost")
                        .EnableCookiesForAllSubdomains();
                case 11:
                    // NOTE: same as 10
                    return Ok()
                        .AddCookies(cookieDescriptors, ".localhost");
                default:
                    return BadRequest();
            }
        }
    }
}
