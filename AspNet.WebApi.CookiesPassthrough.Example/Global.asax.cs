using System.Web;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough.Example
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
