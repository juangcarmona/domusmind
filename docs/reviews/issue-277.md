# Review: #277 — Adopt Lighthouse workflows and pilot the development BFF

Date: 2026-10-28

## Outcome

[PR #278](https://github.com/juangcarmona/domusmind/pull/278) merged the pinned Lighthouse installation, selected workflow projections, optional visual token mapping, and development-only OIDC BFF. [PR #290](https://github.com/juangcarmona/domusmind/pull/290) repaired the worker checkout collision. The owner accepted the post-merge [router run 40657381431](https://github.com/juangcarmona/domusmind/actions/runs/40657381431), [worker run 40657428095](https://github.com/juangcarmona/domusmind/actions/runs/40657428095), and [validated result artifact 12801949947](https://github.com/juangcarmona/domusmind/actions/runs/40657428095/artifacts/12801949947), then closed #277 with `status:done`. The worker published a passed `refine #281` result on the dedicated runner. Production OIDC remains #281.

## What worked

- The one-item integration exception kept #277 open until the default-branch router and worker produced real evidence; code and CI approval preceded the merge.
- The BFF browser journey exercised Keycloak, Aspire, Vite, an authenticated read and mutation, CSRF, session expiry, identity mismatch, and logout revocation. PR #278 links the exact-head CI and browser artifacts.
- The router and worker published a validated stage result on the dedicated runner. The repository variable is active; its current state must be checked when operating the workflow.

## What did not

- The initial operator profile lacked a default model assignment for `refine`. Two worker runs failed before the profile was corrected and the runner restarted.
- The pinned Lighthouse worker templates checked their harness into `tools/`, colliding with tracked DomusMind files. A third run exposed the failure; PR #290 moved the checkout to `.lighthouse-harness/` and regenerated the workflow locks. Future installer updates can overwrite this local correction.
- Comments on a closed PR triggered two router attempts. Both failed closed without dispatching a worker; the variable was disabled while their origin was checked, then re-enabled.

## Change next time

Check that the operator profile resolves every selected stage and run a tracked-`tools/` checkout fixture before activating a newly installed worker. Preserve the `.lighthouse-harness/` correction until Lighthouse's source templates are fixed. No change to the lifecycle policy is proposed; the exception applied only to #277.

## Effort

Not captured. No verified time or token-usage telemetry was available for this item.

## Amendments

None. The remaining production identity work is tracked in #281; [Lighthouse #178](https://github.com/juangcarmona/lighthouse/issues/178) tracks the installer checkout correction.
