using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNet.WebApi.CookiesPassthrough
{
    /// <inheritdoc />
    /// <summary>
    /// Adds cookie headers to the response
    /// </summary>
    public class CookieActionResult : IHttpActionResult
    {
        private readonly IEnumerable<CookieDescriptor> _cookieDescriptors;
        private readonly IHttpActionResult _innerResult;
        private readonly string _domain;
        private bool _isEnabledForAllSubdomains;

        /// <summary>
        /// Adds cookie headers to the response
        /// </summary>
        /// <param name="innerResult">Result to decorate</param>
        /// <param name="cookieDescriptors">Cookie descriptors</param>
        /// <param name="domain">Domain for given cookie descriptors</param>
        public CookieActionResult(
            IHttpActionResult innerResult,
            IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain)
        {
            _innerResult = innerResult;
            _cookieDescriptors = cookieDescriptors;
            _domain = domain;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _innerResult.ExecuteAsync(cancellationToken);

            var headers = _cookieDescriptors
                .Distinct()
                .ToHttpHeaders(_domain, _isEnabledForAllSubdomains);

            foreach (var h in headers)
            {
                response.Headers.Add("Set-Cookie", h);
            }

            return response;
        }

        /// <summary>
        /// Enables cookies for all subdomains
        /// </summary>
        /// <returns></returns>
        public CookieActionResult EnableCookiesForAllSubdomains()
        {
            _isEnabledForAllSubdomains = true;
            return this;
        }
    }
}
