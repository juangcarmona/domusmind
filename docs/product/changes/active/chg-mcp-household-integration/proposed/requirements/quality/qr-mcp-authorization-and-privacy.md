---
id: QR-MCP-AUTHORIZATION-AND-PRIVACY
type: quality-requirement
title: Preserve household authorization and privacy for assistant integrations
status: draft
quality-attribute: security
applies-to:
  - UC-MCP-USE-HOUSEHOLD-CAPABILITIES
verification:
  - scenario: A token issued for another resource, expired or revoked token, insufficient scope, unknown client, or user outside the selected household cannot retrieve household data or cause a state change.
  - scenario: A member with access to one household cannot use an assistant grant to access a different household or another member's restricted information.
  - scenario: An MCP capability invokes the existing application query or command and does not access persistence directly or accept a caller token intended for another service.
  - scenario: Security and audit records identify the authenticated member, client, household, capability, outcome, and correlation without recording bearer tokens, credentials, or sensitive payloads.
---

## Requirement

Assistant integration MUST distinguish user authentication, the client's OAuth grant, and DomusMind's current household/member authorization. Remote integrations MUST use a standards-based OAuth 2.1 and OpenID Connect direction, including Authorization Code with PKCE, protected-resource and authorization-server discovery, audience-bound access tokens, bounded token lifetime, revocation, and least-privilege read/write authority. A valid grant MUST NOT substitute for current membership, member access, or domain authorization.

DomusMind MUST NOT accept or forward a token issued for another resource as an MCP credential. Capabilities MUST not access persistence directly, duplicate domain logic, or allow a client to choose a household/member beyond the authenticated user's authorized scope. Operations initiated by an external client MUST be auditable without logging secrets or unnecessary household content.

## Measurement

- Automated tests cover cross-household and cross-member isolation, revoked/expired/wrong-audience tokens, insufficient scopes, authorization changes after consent, and denial without side effects.
- Code and contract review confirms all data access flows through existing application operations and current domain authorization.
- Audit evidence identifies who, which client, which operation, which household context, and its outcome without bearer-token or sensitive-payload leakage.
