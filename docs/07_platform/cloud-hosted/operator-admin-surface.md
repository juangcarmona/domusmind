# CloudHosted — Operator Admin Surface

## Status

Implemented — V1 minimum operator surface

---

## Purpose

Provide the operator with a minimal management surface to provision and inspect the
CloudHosted deployment without exposing household member data beyond what is necessary
and without diverging from the household product.

---

## Access Model

- the operator admin surface is accessible only to authenticated users with the `operator` role
- the `operator` role is assigned at system initialization (`POST /api/setup/initialize`)
  and via the `BootstrapAdmin` headless seed path
- normal household users never see or access this surface
- route: `/admin`

---

## V1 Capabilities

### Diagnostics panel

- deployment mode
- household count
- user count
- pending invitation count
- system initialization status

### Households

- list all households (id, name, created date, member count)

### Users

- list all registered users (id, email, display name, disabled status, last login)
- disable a user account
- re-enable a disabled user account

### Operator Invitations

- list invitations (email, status, creation date, expiry, created by)
- create a new invitation (email, optional note, 7-day expiry by default)
- revoke a pending invitation

---

## Invitation Model

An operator invitation is account-scoped. It is not the same as a family-member
invitation (InviteMember). It represents the operator's intent to allow a specific
email address to register.

States: `Pending` → `Accepted` | `Revoked` | `Expired` (derived, if now > ExpiresAtUtc)

Token: a cryptographically random URL-safe string to be delivered out-of-band.
Token-based invitation acceptance flow is deferred to V2.

---

## Authorization

- route `/admin/*` requires `isOperator === true` on the authenticated user
- API `/api/admin/*` requires `[Authorize(Policy = "Operator")]`
- the `Operator` policy requires the `operator` role claim in the bearer token

---

## Intentionally Deferred

- token-based registration acceptance flow
- email delivery of invitation tokens
- account deletion / GDPR export
- per-household access revocation
- refresh token revocation from the admin UI
- audit log surface
- multi-operator support
