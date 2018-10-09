using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    internal class CookieActionResult : IHttpActionResult
    {
        private readonly IEnumerable<CookieDescriptor> _cookieDescriptors;
        private readonly IHttpActionResult _innerResult;
        private readonly bool _forAllSubdomains;
        private readonly string _domain;

        public CookieActionResult(
            IHttpActionResult innerResult,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain,
            bool forAllSubdomains = true)
        {
            _innerResult = innerResult;
            _cookieDescriptors = cookieDescriptors;
            _domain = domain;
            _forAllSubdomains = forAllSubdomains;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _innerResult.ExecuteAsync(cancellationToken);

            _cookieDescriptors
                .ToHttpHeaders(_domain, _forAllSubdomains)
                .ToList()
                .ForEach(h => response.Headers.Add("Set-Cookie", h));

            return response;
        }
    }
}
