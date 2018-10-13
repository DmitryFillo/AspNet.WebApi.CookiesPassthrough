using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string ToHttpHeader(this CookieDescriptor cookieDescriptor, string domain, bool forAllSubdomains = false)
        {
            string domainPart;

            if (!string.IsNullOrEmpty(domain))
            {
                domain = forAllSubdomains ? $".{Regex.Replace(domain, @"^www\.", "")}" : domain;

                // NOTE: ".localhost" will not work in the browsers
                domain = string.Equals(domain, ".localhost") ? "localhost" : domain;

                domainPart = $"domain={domain}; ";
            }
            else
            {
                domainPart = "";
            }

            var expiresPart = cookieDescriptor.Expires == DateTime.MinValue ? "" : $"expires={cookieDescriptor.Expires:R}; ";
            var httpOnlyPart = cookieDescriptor.HttpOnly ? "HttpOnly; " : "";

            return $"{cookieDescriptor.Name}={HttpUtility.UrlDecode(cookieDescriptor.Value)}; {expiresPart}{httpOnlyPart}{domainPart}path=/";
        }
    }
}
