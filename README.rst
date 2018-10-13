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

- TBD

TODO:  addcookies encodes, http context (not elegant to mock + do not do autodecode), set clears cookies from httpresponsemessage, also localhost support + fluent; add " " quotes

How to use
==========

TBD
