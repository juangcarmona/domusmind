---
id: QR-MCP-SAFE-AI-OPERATIONS
type: quality-requirement
title: Make assistant-initiated household operations predictable and safe
status: draft
quality-attribute: safety
applies-to:
  - UC-MCP-USE-HOUSEHOLD-CAPABILITIES
verification:
  - scenario: Ambiguous household member, local date/time, timezone, location, list, or target operation causes a clarifying question or a safe refusal rather than a guessed state change.
  - scenario: A consequential change cannot be committed until the member has reviewed and confirmed its resolved details through a supported confirmation path.
  - scenario: Retrying the same confirmed operation does not silently create duplicate plans, tasks, or list items.
  - scenario: Untrusted text returned from household content is treated as data and cannot alter tool permissions or invoke an unrelated action.
---

## Requirement

Assistant-facing operations MUST validate bounded inputs and report structured, non-sensitive errors. Natural-language dates and times MUST be resolved using the household/member locale and timezone where known, with unresolved ambiguity surfaced to the member. The product MUST prevent duplicate effects on retries, require explicit confirmation for consequential actions, and treat household content and external content as untrusted data rather than instructions that can change authorization or capability selection. Rate limits, operational telemetry, and audit trails MUST support safe operation without collecting secrets or unnecessary private content.

## Measurement

- Contract and integration tests cover malformed inputs, ambiguous and invalid dates/times, missing members or locations, domain validation failures, retries, denial, and required confirmation.
- Security tests demonstrate that untrusted content cannot expand granted capabilities, change household context, or bypass user confirmation and domain rules.
- Operational evidence shows bounded request rates, structured errors, and useful request correlation while excluding tokens, credentials, and private payloads.
