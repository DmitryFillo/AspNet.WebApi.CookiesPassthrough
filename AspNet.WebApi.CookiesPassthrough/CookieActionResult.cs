using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    public class CookieActionResult : IHttpActionResult
    {
        private readonly IEnumerable<CookieDescriptor> _cookieDescriptors;
        private readonly IHttpActionResult _innerResult;
        private readonly string _domain;
        private bool _forAllSubdomains;

        public CookieActionResult(
            IHttpActionResult innerResult,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain)
        {
            _innerResult = innerResult;
            _cookieDescriptors = cookieDescriptors;
            _domain = domain;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _innerResult.ExecuteAsync(cancellationToken);

            _cookieDescriptors
                .Distinct()
                .ToHttpHeaders(_domain, _forAllSubdomains)
                .ToList()
                .ForEach(h => response.Headers.Add("Set-Cookie", h));

            return response;
        }

        public CookieActionResult EnableCookiesForAllSubdomains()
        {
            _forAllSubdomains = true;
            return this;
        }
    }
}
