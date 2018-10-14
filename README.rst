==================================
ASP.NET Web API CookiesPassthrough
==================================

.. image:: https://travis-ci.com/DmitryFillo/AspNet.WebApi.CookiesPassthrough.svg?branch=master
     :target: https://travis-ci.com/DmitryFillo/AspNet.WebApi.CookiesPassthrough


Allows you to add cookies for IHttpActionResult in WebAPI controllers.

.. contents::

Motivation
==========

There are several ways to adds cookies to the response in WebAPI. The recommended way, according to the `docs <https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-cookies#cookies-in-web-api>`_, is to use ``resp.Headers.AddCookies(cookies)`` extension method, but here is some disadvantages:

- Cookie values are always encoded. It's `complicated topic <https://stackoverflow.com/questions/1969232/allowed-characters-in-cookies>`_, so encoding or decoding should be configurable.
- Array will be encoded if you'll try to set it via passing string. ``CookieHeaderValue`` supports ``NameValueCollection`` as values and such cookies will be presented as ``cookie-name=key1=value1&key2=value2``. But what to do if you want to just simply pass such string to ``CookieHeaderValue`` and you don't want to encode ``=``? It useful for cases when you passing cookie values through services, e.g. integration with legacy APIs. 

TODO:  http context (not elegant to mock + do not do autodecode), set clears cookies from httpresponsemessage, also localhost support + fluent on IHttpActionResult

How to use
==========

TBD

Special thanks to
=================

TBD

