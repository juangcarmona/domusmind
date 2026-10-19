---
id: CHG-MCP-HOUSEHOLD-INTEGRATION-001
type: product-change
title: Connect external AI assistants to household capabilities through MCP
status: draft
base-revision: 'b4f77ad01ca87d2de33682d24ea2b7eee9e4cb15'
operations:
  add:
    - UC-MCP-USE-HOUSEHOLD-CAPABILITIES
    - FR-MCP-EXPOSE-HOUSEHOLD-CAPABILITIES
    - QR-MCP-AUTHORIZATION-AND-PRIVACY
    - QR-MCP-SAFE-AI-OPERATIONS
  modify: []
  remove: []
---

Schema reference: docs/specification/frontmatter-reference.md#product-change

## Problem

Household members can coordinate using DomusMind's existing household capabilities, but compatible external AI assistants do not have a supported, least-privilege way to discover and use those capabilities in natural language. Members must switch into the application and translate a question or request into a product-specific interaction. The product definition does not yet express an interoperable assistant entry point or the additional consent, authorization, safety, and accountability boundaries it requires.

## Intended Product Outcome

An authenticated household member can connect a compatible AI assistant to DomusMind and ask natural-language questions or request supported household actions. The assistant can retrieve information the member is entitled to see and, where the member has granted the relevant authority, perform explicitly supported changes. DomusMind remains the source of truth: each operation is owned and validated by its existing product area, and an AI client cannot create a parallel household model or bypass its rules.

The initial value should come from a small, coherent set of common household workflows, with read access established before carefully bounded writes. Candidate early reads are the member or household agenda, pending tasks, and the current household member directory needed to resolve names safely. Candidate later reads include household lists, meal plans, routines, and responsibility areas. Candidate writes include creating or changing plans and tasks and adding an item to an existing list. The delivery plan must assess each candidate against user value, existing application support, domain ownership, permission, ambiguity, confirmation, effort, and sequencing; this list is neither exhaustive nor a mandatory tool catalog.

## Rationale

Household coordination is often expressed as short questions and instructions, and natural-language access can reduce friction while reusing capabilities already provided by DomusMind. An interoperability interface is valuable only if it preserves household trust: authorization must be tied to the authenticated member and current household membership, writes must be explicit and reviewable, and data must continue to be governed by the existing domain contexts.

The current MCP specification defines remote HTTP authorization, protected-resource metadata, authorization-server discovery, audience-specific tokens, and a distinct STDIO credential model. Its current revision prefers Client ID Metadata Documents, while retaining Dynamic Client Registration for compatibility. OAuth 2.1 remains an Internet-Draft; OpenID Connect discovery can provide the authorization-server metadata mechanism. These references guide the delivery assessment, not a claim that every client implements every optional mechanism: [MCP specification](https://modelcontextprotocol.io/specification/2026-07-28), [MCP authorization](https://modelcontextprotocol.io/specification/2026-07-28/basic/authorization), [MCP security practices](https://modelcontextprotocol.io/docs/2026-07-28/tutorials/security/security_best_practices), [official .NET SDK](https://github.com/modelcontextprotocol/csharp-sdk), [OAuth 2.1 draft](https://datatracker.ietf.org/doc/draft-ietf-oauth-v2-1/), [OpenID Connect Core](https://openid.net/specs/openid-connect-core-1_0.html), [Protected Resource Metadata (RFC 9728)](https://www.rfc-editor.org/rfc/rfc9728.html), [Authorization Server Metadata (RFC 8414)](https://www.rfc-editor.org/rfc/rfc8414.html), and [PKCE (RFC 7636)](https://www.rfc-editor.org/rfc/rfc7636.html).

## Affected Product Areas

- Household member access to shared agenda, tasks, lists, meal plans, routines, areas, and household membership information.
- Calendar-owned plans and schedules; Tasks-owned tasks and routines; Lists-owned lists and items; Meal Planning-owned meal plans; Responsibilities-owned areas; and Family-owned household membership and identity resolution.
- A product-level external assistant connection, consent, and operation history that identifies the member and client responsible for an action without making MCP a source of truth.
- Existing unified read models and context-owned write operations remain authoritative. MCP exposes meaningful operations over them; it does not duplicate domain rules or expose persistence structures.

## Open Questions

- Should the authorization server be an extension of DomusMind's built-in identity module, a separately operated standards-based provider, or support both? Current product architecture uses built-in local authentication and does not establish a general external OAuth authorization server.
- What deployment shape best preserves the existing single-deployable self-hosting experience while providing a remotely reachable, isolated MCP resource: an endpoint in the API host, or a separate component? This requires an architecture and operational assessment.
- How should a household member explicitly confirm a consequential write when the connected client does not offer a consistent confirmation surface? Which writes can be considered low-impact enough for an individually granted write scope?
- How should OAuth grants identify a household when one user belongs to multiple households, and how should changes to membership, role, or member access invalidate or narrow an existing grant?
- Should scoped Personal Access Tokens be supported for trusted local agents and development, and if so, what lifetime, storage, rotation, revocation, and restrictions are acceptable? They are not the target for remote integrations.
- Which clients and client plans/configurations are in the supported initial interoperability promise, given differences in HTTP transport, OAuth registration, private-network connectivity, write approvals, and local STDIO support?

## Product Acceptance

- The Product Definition names the household member and external AI client as actors and describes the question-and-action journey, including consent, household/member scope, and an understandable outcome when access is denied or information is ambiguous.
- Product requirements distinguish read access from state-changing access and require each operation to remain within the invoking member's current household permissions and existing domain rules.
- The initial capability set is prioritized from repository evidence rather than copied mechanically from API endpoints; each proposed capability has its value, owning context, supporting query/command, permissions, read/write class, ambiguity and confirmation needs, effort, and phase recorded in the delivery plan.
- The product definition makes clear that MCP is an interoperability interface, not a domain, data store, LLM dependency, or autonomous agent framework.
- Authorization/consent, security and safety expectations, success criteria, out-of-scope areas, unresolved decisions, and impacts on existing specifications are explicit and reviewable while this change remains a draft.
- Existing domain specifications remain canonical; accepted Product Change review determines which context specifications need focused MCP-facing contracts, without duplicating their domain behavior.

## Out of Scope

- Implementing an MCP endpoint, OAuth authorization server, client, tool, Resource, Prompt, or transport.
- Selecting a final host/deployment topology, identity provider, frontend, or agent-orchestration framework before the architecture assessment.
- Replacing or modifying the authoritative domain model, read models, context-owned commands, queries, invariants, or application-layer authorization.
- Exposing every API operation, raw database records, authentication credentials, calendar-provider credentials, arbitrary household administration, or unreviewed destructive operations.
- Making an AI model or external assistant a required DomusMind dependency, or granting an assistant autonomous authority beyond the member's explicit, revocable grant.
