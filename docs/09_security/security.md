# DomusMind — Security

## Purpose

This document defines the baseline security model for DomusMind V1.

Security in V1 focuses on:

- authentication
- authorization
- family data isolation
- secret handling
- secure defaults

---

## Authentication

DomusMind V1 uses **local authentication implemented inside the modular monolith**. 

Rules:

- authentication is part of the application boundary
- user identity is managed internally
- external identity providers are optional future integrations

Important distinction:

- `User` is an authentication concept
- `Member` is a family domain concept

These must remain separate. 

---

## Authorization

Authorization must be family-scoped.

Rules:

- a user may access only the families they belong to or administer
- commands and queries must enforce family isolation
- cross-family access is forbidden by default

V1 authorization can remain simple, but isolation must be strict.

---

## Session Model

DomusMind V1 uses token-based authentication.

Recommended approach:

- JWT bearer tokens
- short-lived access tokens
- refresh token support optional in V1

Rules:

- tokens must contain user identity
- tokens must not embed domain state
- authorization checks always verify family membership

Authentication identity and domain identity remain separate:

User → authentication identity  
Member → family domain identity

---

## Authorization Model

Authorization is family-scoped.

Typical rules:

- a user may belong to multiple families
- commands must verify membership in the target family
- administrative operations require elevated privileges

Authorization enforcement occurs at:

- API boundary
- application slice handlers

Domain entities remain authorization-agnostic.

---

## Domain Boundary

Authentication and authorization must not pollute the core domain model.

Rules:

- domain entities do not depend on auth framework types
- access checks occur in API, application, or policy boundaries
- family membership in the domain is not the same as login identity

---

## Secret Management

V1 secret handling principles:

- secrets must never be committed to source control
- CI secrets are stored in GitHub Actions secrets
- local development secrets should use environment variables or local secret storage
- no cloud secret manager is required yet

Examples of secrets:

- database connection strings
- signing keys
- notification credentials
- external integration tokens

---

## Transport Security

Production deployments must use HTTPS.

Rules:

- no plaintext auth traffic
- secure cookies or bearer tokens only
- Swagger access may be restricted outside development

---

## Password and Credential Handling

Rules:

- passwords must be hashed using a modern password hashing algorithm
- plaintext passwords must never be stored or logged
- credential reset flows must be explicit and auditable

---

## Auditability

Security-relevant actions should be traceable.

Examples:

- login
- failed login
- family creation
- ownership transfer
- permission-sensitive changes

Domain events and application logs together provide the baseline audit trail. 

---

## Data Protection

Household data is sensitive.

Minimum expectations:

- family-level isolation
- least privilege by default
- secure persistence configuration
- controlled exposure in logs and telemetry

Sensitive fields should not be unnecessarily returned or logged.

---

## Future Considerations

Possible later additions:

- external identity providers
- MFA
- device/session management
- per-family roles
- encrypted secret stores
- fine-grained audit dashboards

These are not required for V1.

---

## Summary

DomusMind V1 uses local authentication, strict family-level isolation, internal secret handling discipline, and secure defaults.

The key architectural rule remains: authentication identity and household domain identity are separate concepts.
