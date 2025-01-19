# ASP.NET Core application access token management

Managing application access tokens in an ASP.NET Core web application. Any application with or without a user can use application access tokens as long as the application can persist the tokens in a safe way.

## Setup 

An ASP.NET Core web application authenticates using OpenID Connect and OpenIddict as the secure token server. The application needs to use data from an app-to-app resource. An OAuth client credential flow is used to get an application access token to access the API. The OAuth client credentials flow can only be used when it can keep a secret. This token has nothing in common with the delegated access token from the user authentication. The application is persisted once for the application. An in-memory cache is used for this. The application sends the application access token as a bearer token to the API.

![ASP.NET Core application access token management](https://github.com/damienbod/token-mgmt-ui-application/blob/main/images/context.png)

## Blogs in this series

- [ASP.NET Core user delegated access token management](https://damienbod.com/2025/01/15/asp-net-core-user-delegated-access-token-management/)
- [ASP.NET Core user application access token management](https://damienbod.com)

## Links

https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/additional-claims

https://github.com/dotnet/aspnetcore/issues/8175

https://www.epochconverter.com/

## Standards

[JSON Web Token (JWT)](https://datatracker.ietf.org/doc/html/rfc7519)

[The OAuth 2.0 Authorization Framework](https://datatracker.ietf.org/doc/html/rfc6749)

[OAuth 2.0 Demonstrating Proof of Possession DPoP](https://datatracker.ietf.org/doc/html/rfc9449)

[OAuth 2.0 JWT-Secured Authorization Request (JAR) RFC 9101](https://datatracker.ietf.org/doc/rfc9101/)

[OAuth 2.0 Mutual-TLS Client Authentication and Certificate-Bound Access Tokens](https://datatracker.ietf.org/doc/html/rfc8705)

[OpenID Connect 1.0](https://openid.net/specs/openid-connect-core-1_0-final.html)

[Microsoft identity platform and OAuth 2.0 On-Behalf-Of flow](/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow)

[OAuth 2.0 Token Exchange](https://datatracker.ietf.org/doc/html/rfc8693)

[JSON Web Token (JWT) Profile for OAuth 2.0 Access Tokens](https://datatracker.ietf.org/doc/html/rfc9068)

[HTTP Semantics RFC 9110](https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2)
