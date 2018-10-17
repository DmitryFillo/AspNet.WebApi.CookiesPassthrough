==================================
ASP.NET Web API CookiesPassthrough
==================================

.. image:: https://travis-ci.com/DmitryFillo/AspNet.WebApi.CookiesPassthrough.svg?branch=master
     :target: https://travis-ci.com/DmitryFillo/AspNet.WebApi.CookiesPassthrough

TBD: 

- iconUrl 
- review docs and code
- nuget pack MyProject.csproj -properties Configuration=Release 

Allows you to add cookies for IHttpActionResult in WebAPI controllers.

.. contents::

Motivation
==========

There are several ways to add cookies to the response in WebAPI. The recommended way, according to the `docs <https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-cookies#cookies-in-web-api>`_, is to use ``resp.Headers.AddCookies(cookies)`` extension method, but there are some disadvantages:

- Cookie values are always encoded. It's `complicated topic <https://stackoverflow.com/questions/1969232/allowed-characters-in-cookies>`_, so encode / decode should be configurable, e.g. Chrome works well with spaces in cookie values or sometimes you need ``=`` char in a cookie value.
- ``CookieHeaderValue`` `supports name-value pairs<https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-cookies#structured-cookie-data>`_ and such collections will be presented as ``cookie-name=key1=value1&key2=value2``, but collection will be encoded if you'll try to set it via just passing string. Passing string is useful for cases when you passing cookie values through services, e.g. integration with legacy cookie-based APIs.

Another way is to set cookies on `HttpResponse.Cookies <https://docs.microsoft.com/en-us/dotnet/api/system.web.httpresponse.cookies?view=netframework-4.7.2#System_Web_HttpResponse_Cookies>`_ via ``HttpContext`` (check `example <https://stackoverflow.com/questions/9793591/how-do-i-set-a-response-cookie-on-httpreponsemessage/9793779#9793779>`_), but there are even more serious disadvantages:

- Using ``HttpContext`` in WebAPI is bad practice, because you cannot get them in self host.
- Potential problems with ``new Thread()``.
- Harder to mock.
- `Complicated behaviour <https://stackoverflow.com/questions/8491075/why-does-httpcontext-response-cookiesfoo-add-a-cookie>`_.

Better to have simple API for ``IHttpActionResult`` w/o described disadvantages. Also good to have `localhost support <https://stackoverflow.com/questions/1134290/cookies-on-localhost-with-explicit-domain>`_ or "enable these cookies for all subdomains" feature out-the-box.

How to use
==========

You can install ``AspNet.WebApi.CookiesPassthrough`` package via nuget.

.. code:: c#

  var cookieDescriptors = new[] 
  {
       // simple cookie with Path=/
       new CookieDescriptor("test-cookie", "1"),
       
       // encode
       new CookieDescriptor("test-cookie2", "2=") {
           CodeStatus = CookieCodeStatus.Encode
       },
        
       // expire, secure, httponly + decode
       new CookieDescriptor("test-cookie3", "a%3D3") {
           Secure = true,
           CodeStatus = CookieCodeStatus.Decode,
           HttpOnly = true,
           Expires = new DateTime(2118, 1, 1)
       },
        
       // path will be added and no decode or encode
       new CookieDescriptor("test-cookie4", "4%3D=") {
           Path = "/subfolder/"
       },
   };

   // also you can use Request.GetReferrerHost() to get referrer's host which is useful when you're developing AJAX API
   return Ok().AddCookies(cookieDescriptors, Request.GetRequestHost());

You can enable cookies for all subdomains:

.. code:: c#
   
   // domain will be ".example.org"
   return Ok().AddCookies(cookieDescriptors, "example.org").EnableCookiesForAllSubdomains();
   
   // same, domain will be ".example.org"
   return Ok().AddCookiesForAllSubdomains(cookieDescriptors, "www.example.org");
   
   // or even this
   return Ok()
       .AddCookiesForAllSubdomains(cookieDescriptorsForAllSubdomains, "example.org")
       .AddCookies(cookieDescriptorsForOneDomain, "example.com")
       .AddCookies(cookieDescriptorsForAnotherDomainAndAllSubdomains, "www.example.net")
       .EnableCookiesForAllSubdomains();

If domain is localhost
======================

`Browser has problems with localhost cookies <https://stackoverflow.com/questions/1134290/cookies-on-localhost-with-explicit-domain>`_. If you'll specify domain as ``localhost`` or even ``.localhost`` it will not be added to the response at all to make cookies works with localhost for almost all browsers.

Enable cookies for all subdomains
=================================

When you call ``.EnableCookiesForAllSubdomains()`` or use ``.AddCookiesForAllSubdomains(...)`` the following logic domain convertion will be applied:

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

Play with examples
==================

Check ``AspNet.WebApi.CookiesPassthrough.Example`` project.

Special thanks to
=================

Thanks to `rustboyar <https://github.com/rustboyar>`_ and `niksanla2 <https://github.com/niksanla2>`_. These guys faced some issues with cookies (related with encoding) in WebAPI when trying to send them back from legacy API and developed PoC. I decided to research the topic a bit and create this package to make common "cookiejob" simple.

