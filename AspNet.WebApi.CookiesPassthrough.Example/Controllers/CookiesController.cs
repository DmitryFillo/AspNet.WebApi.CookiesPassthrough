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
                new CookieDescriptor("test-cookie", "test-cookie"),
                new CookieDescriptor("test-cookie-id", id.ToString()),
            };

            return Ok(new { id }).AddCookies(cookieDescriptors, Request.GetClientHost());
        }
    }
}
