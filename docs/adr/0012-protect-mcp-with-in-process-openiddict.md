---
name: protect-mcp-with-in-process-openiddict
description: Protect the remote MCP resource with an in-process OpenIddict OAuth/OIDC boundary.
status: Proposed
date: 2026-10-20
deciders: []
tags:
  - authentication
  - authorization
  - mcp
  - oauth
---

# ADR-0012: Protect MCP with in-process OpenIddict

## Context

DomusMind's existing identity module authenticates local users and issues API JWTs, but it is not an OAuth 2.1/OIDC authorization server. The remote MCP resource needs a separate standards-based authorization boundary with Authorization Code + PKCE, protected-resource metadata, issuer and resource validation, token storage and revocation, and household-scoped consent. The boundary must continue to resolve a trusted DomusMind `AuthUser` and defer household and member authorization to the existing application services.

The first implementation must remain self-hostable and avoid introducing an external identity-provider dependency before account linking, client registration, consent ownership, and deployment reachability are defined. Production key management, interoperability coverage, and the final consent experience remain release gates.

## Decision

We will host the MCP OAuth/OIDC authorization server and resource validation in the existing `DomusMind.Api` deployment using OpenIddict 7.7.2 and its Entity Framework Core stores. OpenIddict authorization, consent, and token records are infrastructure records; they do not replace `AuthUser`, household membership, or application authorization.

The remote resource will require the exact configured MCP resource URI and the `mcp.read` scope for this slice. Access tokens will be short-lived, resource-bound, validated against stored token state, and mapped to an existing `AuthUser`; every capability will still perform current household and member authorization. The existing local API JWT scheme will remain separate and will not be accepted as an MCP bearer token.

This record is Proposed until production key management, consent UX, client interoperability, and the security review are accepted for release.

## Consequences

- **Positive:** The first remote integration remains self-hostable, reuses the current user and application authorization boundary, and uses a maintained OAuth/OIDC implementation rather than custom protocol code.
- **Negative:** DomusMind owns authorization-server operations, key rotation, token persistence, consent behavior, abuse controls, and the security consequences of sharing the API process.
- **Neutral:** An external provider or separate authorization service can be evaluated later without making it the MCP domain boundary. The current implementation must not expose household tools until its resource and domain authorization gates pass.

## Alternatives considered

- **External OAuth/OIDC provider as the default:** rejected for this slice because it adds account linking, provider availability, consent ownership, and self-hosted deployment dependencies before those contracts are established.
- **Custom OAuth/OIDC implementation:** rejected because standards behavior, token storage, PKCE, discovery, and revocation should remain in a maintained protocol framework.
- **Accept the existing DomusMind API JWT:** rejected because it has a different audience and no OAuth scope or resource-indicator contract.

## References

- Related: [issue #229](https://github.com/juangcarmona/domusmind/issues/229), [OAuth/OIDC delivery plan](../delivery/plans/issue-229-mcp-oauth-authorization.md), [ADR-0002](0002-keep-authentication-local-and-separate-from-member-identity.md), [ADR-0011](0011-expose-mcp-through-an-in-process-sdk-endpoint.md).
- [OpenIddict introduction](https://documentation.openiddict.com/introduction)
- [OpenIddict ASP.NET Core integration](https://documentation.openiddict.com/integrations/aspnet-core)
- [RFC 9728: OAuth 2.0 Protected Resource Metadata](https://www.rfc-editor.org/rfc/rfc9728.html)
- [RFC 8707: Resource Indicators for OAuth 2.0](https://www.rfc-editor.org/rfc/rfc8707.html)
