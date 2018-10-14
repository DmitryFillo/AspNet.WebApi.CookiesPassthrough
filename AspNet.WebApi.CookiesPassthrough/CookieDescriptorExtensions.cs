using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AspNet.WebApi.CookiesPassthrough
{
    internal static class CookieDescriptorExtensions
    {
        public static IEnumerable<string> ToHttpHeaders(
            this IEnumerable<CookieDescriptor> cookieDescriptors,
            string domain, bool forAllSubdomains = false) =>
            cookieDescriptors.Select(cd => cd.ToHttpHeader(domain, forAllSubdomains));

        // TBD: use string builder
        public static string ToHttpHeader(this CookieDescriptor cookieDescriptor, string domain, bool forAllSubdomains = false)
        {
            var result = new StringBuilder($"{cookieDescriptor.Name}=");

            switch (cookieDescriptor.CodeStatus)
            {
                case CookieCodeStatus.Decode:
                    result.Append(HttpUtility.UrlDecode(cookieDescriptor.Value));
                    break;
                case CookieCodeStatus.Encode:
                    result.Append(HttpUtility.UrlEncode(cookieDescriptor.Value));
                    break;
                case CookieCodeStatus.Nothing:
                    result.Append(cookieDescriptor.Value);
                    break;
                default:
                    result.Append(cookieDescriptor.Value);
                    break;
            }
            result.Append("; ");

            if (cookieDescriptor.Expires != default(DateTime))
            {
                result.Append($"expires={cookieDescriptor.Expires:R}; ");
            }

            if (cookieDescriptor.HttpOnly)
            {
                result.Append("HttpOnly; ");
            }

            if (cookieDescriptor.Secure)
            {
                result.Append("Secure; ");
            }

            if (!string.IsNullOrEmpty(domain))
            {
                domain = forAllSubdomains && domain[0] != '.' ? $".{Regex.Replace(domain, @"^www\.", "")}" : domain;

                // NOTE: ".localhost" will not work in the browsers
                domain = string.Equals(domain, ".localhost") ? "localhost" : domain;

                result.Append($"domain={domain}; ");
            }

            result.Append(string.IsNullOrEmpty(cookieDescriptor.Path) ? "path=/" : $"path={cookieDescriptor.Path}");

            return result.ToString();
        }
    }
}
