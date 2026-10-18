---
id: QR-ASSISTANT-PROVIDER-PRIVACY
type: quality-requirement
title: Protect household information sent to assistant providers
status: draft
quality-attribute: security
applies-to:
  - UC-CHAT-ASK-ASSISTANT
  - UC-ASSISTANT-CONFIGURE-PROVIDERS
verification:
  - scenario: Assistant context contains no information unavailable to the invoking member.
  - scenario: Members can identify which target handles a request and whether fallback occurred.
  - scenario: A member can inspect provider data-sharing boundaries before using the assistant.
uses-terms:
  - TERM-ASSISTANT-PROVIDER-TARGET
  - TERM-ASSISTANT-RESPONSE
---

## Requirement

Assistant requests MUST include only information the invoking member is authorized to access and only the context needed for the request. Before use, the product MUST explain whether each configured target is hosted or local and what request text and household context it receives. Each response MUST identify the target that handled it and whether fallback occurred. Provider credentials MUST be treated as secrets and MUST NOT be exposed to ordinary household members.

## Measurement

- A member cannot cause provider context to include data outside that member's existing permissions.
- The configured order and effective provider for an assistant response are visible to household members.
- Provider credentials are never returned in clear text after saving.
