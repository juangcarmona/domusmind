# Review: #279 — Accept production OIDC and household invitation product rules

Date: 2026-10-28

## Outcome

[PR #280](https://github.com/juangcarmona/domusmind/pull/280) merged as `ca23b189bf52e5ab47aead0d54c768fff9c54ca1`. The approved Product Change is applied to the model. [Product CI](https://github.com/juangcarmona/domusmind/actions/runs/40645812075), [architecture docs CI](https://github.com/juangcarmona/domusmind/actions/runs/40645811942), [product snapshot CI](https://github.com/juangcarmona/domusmind/actions/runs/40645812014), and the [engineering-site workflow](https://github.com/juangcarmona/domusmind/actions/runs/40645811913) succeeded on the merge commit. This review does not claim that production authentication has changed.

## What worked

- The owner settled the sign-in, setup, invitation and recovery decisions before the model was applied.
- ProductShape overlay validation exposed a coherent product delta; citation verification then identified the consumers that needed updates.
- The product definition and OpenSpec/architecture alignment shipped separately from production security code.

## What did not

- Applying the model made existing Family and architecture citations stale, and one removed use-case citation unresolved. These were repaired before integration.
- The OpenSpec CLI was absent from the initial environment; validation required a temporary installation.
- The repository still describes the current local-authentication runtime. Production implementation and the ADR replacing ADR-0002 remain open in #281.

## Change next time

Keep product application and consumer citation repair in the same reviewable change. The existing validation checks detected the drift, so no lifecycle amendment is proposed.

## Effort

Not captured. No verified time or token-usage telemetry was available for this item.

## Amendments

None. #281 owns the implementation and architecture decision; #277 owns the development BFF pilot.
