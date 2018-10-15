==================================
ASP.NET Web API CookiesPassthrough
==================================

.. image:: https://travis-ci.com/DmitryFillo/AspNet.WebApi.CookiesPassthrough.svg?branch=master
     :target: https://travis-ci.com/DmitryFillo/AspNet.WebApi.CookiesPassthrough


Allows you to add cookies for IHttpActionResult in WebAPI controllers.

.. contents::

Motivation
==========

There are several ways to add cookies to the response in WebAPI. The recommended way, according to the `docs <https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-cookies#cookies-in-web-api>`_, is to use ``resp.Headers.AddCookies(cookies)`` extension method, but there are some disadvantages:

- Cookie values are always encoded. It's `complicated topic <https://stackoverflow.com/questions/1969232/allowed-characters-in-cookies>`_, so encoding or decoding should be configurable, e.g. Chrome works well with spaces in cookie values or sometimes you need ``=`` char in cookie value.
- You can set arrays in values via special API, but array will be encoded if you'll try to set it via passing string. ``CookieHeaderValue`` supports ``NameValueCollection`` as values and such cookies will be presented as ``cookie-name=key1=value1&key2=value2``. But what to do if you want to just simply pass such string to ``CookieHeaderValue`` and you don't want to encode ``=``? It useful for cases when you passing cookie values through services, e.g. integration with legacy APIs.

Another way is to set cookies on `HttpResponse.Cookies <https://docs.microsoft.com/en-us/dotnet/api/system.web.httpresponse.cookies?view=netframework-4.7.2#System_Web_HttpResponse_Cookies>`_ via ``HttpContext``, check `example <https://stackoverflow.com/questions/9793591/how-do-i-set-a-response-cookie-on-httpreponsemessage/9793779#9793779>`_, but there are even more serious disadvantages:

- Using ``HttpContext`` in WebAPI is bad practice, because you cannot get them in self host.
- Potential problems with ``new Thread()``.
- Harder to mock.
- `Complicated behaviour <https://stackoverflow.com/questions/8491075/why-does-httpcontext-response-cookiesfoo-add-a-cookie>`_.

Better to have simple API for ``IHttpActionResult`` (WebAPI way) w/o described disadvantages. Also good to have `localhost support <https://stackoverflow.com/questions/1134290/cookies-on-localhost-with-explicit-domain>`_ or "enable these cookies for all subdomains" feature out-the-box.

Of course, cookies are legacy and complicated topic, so there is no golden bullet for all cases, just assume that ``AspNet.WebApi.CookiesPassthrough`` can address some set of common cases.

How to use
==========

You can install ``AspNet.WebApi.CookiesPassthrough`` package via nuget.

Then try to add cookies to the response:

.. code:: c#

  var cookieDescriptors = new[] 
  {
       // NOTE: simple cookie with Path=/
       new CookieDescriptor("test-cookie", "1"),
       
       // NOTE: just encode
       new CookieDescriptor("test-cookie2", "2=") {
           CodeStatus = CookieCodeStatus.Encode
       },
        
       // NOTE: expire, secure, httponly + decode
       new CookieDescriptor("test-cookie3", "a%3D3") {
           Secure = true,
           CodeStatus = CookieCodeStatus.Decode,
           HttpOnly = true,
           Expires = new DateTime(2118, 1, 1)
       },
        
       // NOTE: path will be added and no decode or encode
       new CookieDescriptor("test-cookie4", "4%3D=") {
           Path = "/subfolder/"
       },
   };

   // NOTE: also you can use Request.GetReferrerHost() which is useful when you're developing AJAX API
   return Ok().AddCookies(cookieDescriptors, Request.GetRequestHost());

Note ``CookieDescriptor`` allows you to control encode or decode.

You can add dot before domain to enable cookies for all subdomains:

.. code:: c#
   
   // NOTE: domain will be ".example.org"
   return Ok().AddCookies(cookieDescriptors, "example.org").EnableCookiesForAllSubdomains();
   
   // NOTE: same
   return Ok().AddCookiesForAllSubdomains(cookieDescriptors, "example.org");
   
   // NOTE: or even this
   return Ok()
       .AddCookiesForAllSubdomains(cookieDescriptorsForAllSubdomains, "example.org")
       .AddCookies(cookieDescriptorsForOneDomain, "example.org")
       .AddCookies(cookieDescriptorsForAnotherDomainAndAllSubdomains, "example.org")
       .EnableCookiesForAllSubdomains();

If domain is localhost
======================

If you'll specify domain as "localhost" or even ".localhost" it will not be added to the response at all to make cookies works with localhost for almost all browsers.

Enabling cookie for all subdomains
==================================

When you calling ``.EnableCookiesForAllSubdomains()`` or using ``.AddCookiesForAllSubdomains(...)`` the following logic will be applied:

.. code:: c#

  "localhost"        => ""
  ".localhost"       => ""
  "www.localhost"    => ".www.localhost"
  "www.localhost.ru" => ".localhost.ru"
  "www.org"          => ".www.org"
  ".www.org"         => ".www.org"
  "example.org"      => ".example.org"
  "www.example.org"  => ".example.org"
  ".www.example.org" => ".www.example.org"

Example
=======

Check ``AspNet.WebApi.CookiesPassthrough.Example`` project.

Special thanks to
=================

Thanks to `rustboyar <https://github.com/rustboyar>`_ and `niksanla2 <https://github.com/niksanla2>`_. These guys faced some issues with cookies (related with encoding) in WebAPI when trying to send them back from legacy API and developed PoC. I decided to research the topic a bit and create this package to make common "cookiejob" simple.

