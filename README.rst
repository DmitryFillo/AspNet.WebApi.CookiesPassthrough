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

You can install this package via nuget.

TBD

Special thanks to
=================

Thanks to `rustboyar <https://github.com/rustboyar>`_ and `niksanla2 <https://github.com/niksanla2>`_. These guys faced some issues with cookies (related with encoding) in WebAPI when trying to send them back from legacy API and developed PoC. I decided to research the topic a bit and created this package under impression. 

