---
id: BR-ASSISTANT-PROVIDER-FALLBACK
type: business-rule
title: Assistant fallback is ordered and limited to provider failures
status: draft
applies-to:
  - UC-ASSISTANT-CONFIGURE-PROVIDERS
uses-terms:
  - TERM-ASSISTANT-PROVIDER-TARGET
---

## Rule

Assistant Provider Targets are tried in the household manager's configured order, up to three targets. The next target may be tried only when the current target is unavailable, times out, is rate-limited, fails with a server error, or lacks a required capability for the operation. A valid answer, provider refusal, user authorization denial, ambiguous request, or owning-area validation failure MUST NOT trigger provider fallback. The response MUST identify the target that handled the request and disclose when a fallback target was used.

## Rationale

Provider routing must remain predictable and must not conceal where household information was processed (issue #143).

## Examples

- A timeout at the primary target permits trying the configured secondary target and the member sees that fallback occurred.
- A response that asks the member for clarification is a valid result and does not cause a retry at another target.
- A request denied by household authorization is not retried using another provider.

## Exceptions

None.
