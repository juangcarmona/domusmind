# FF run 2d8ff8d9-4e44-44a7-944f-3381af972498

- Work item: [#277](https://github.com/juangcarmona/domusmind/issues/277)
- Branch: `feat/277-lighthouse-adoption`
- Pull request: [#278](https://github.com/juangcarmona/domusmind/pull/278)
- Starting commit: `e206b6b511d8bedd4240e4598ee204bfc0e534ab`
- Executor: GPT-6 (Codex)
- Independent reviewer: `gpt-6.1-terra`
- Correction limit: 3 per gate
- Status: running

## Stage record

### Refine

Completed at 2026-10-28T06:27:14Z. The issue has the required scope, stakeholder, dependencies, product boundaries, risks and observable acceptance criteria. Added explicit Aspire + Vite + Keycloak evidence requirements, including negative session/CSRF/Origin cases. The issue now has exactly one lifecycle label: `status:ready`.

Review debt noted: issues #205 and #226–#232 have no per-item entries under `docs/reviews/`. This debt does not block the current work.

### Propose

Started at 2026-10-28T06:28:00Z. Added `docs/delivery/plans/issue-277.md` with architecture, ordered work, test plan, Definition of Done and known deviations. Independent review by `gpt-6.1-terra` requested changes against commit `e206b6b511d8bedd4240e4598ee204bfc0e534ab` (reviewed plan SHA-256 `f0c71e4a734e0c9eeb3e7c772decabb2107a54b7fa951fb854a54c394ebc3574`). The reviewer required real-browser evidence for expiry, user switching and forwarded Origin/Host behavior, plus verified-email identity matching and explicit mismatch rejection. Correction round 1 adds these requirements; corrected plan SHA-256 is `d2a7a82ffe53dcd66ca32919fd7208721fa3467b54677ba748b48d25c8837b71`. The reviewer approved the corrected plan at commit `9b775074f95536f38d4c993e7a9cafb4d84119f5`, noting that runtime and CI evidence are still required for completion. During implementation design, FF clarified the stronger logout acceptance: a copied pre-logout cookie must be rejected. Correction round 2 updates the plan for a development-only server-side ticket store and records the single-process limitation; amended plan SHA-256 is `5cb907bd039cfbfb7700d762d34414a0d21f1510808fa7f9ccbb1fe9d771513f`. The reviewer approved this amendment against `00dd8b8296020bd01d2b37541f24ee257ea3c6b4`; issue #277 acceptance criteria now explicitly require ticket revocation and copied-cookie rejection. Existing implementation commits predate this FF run; recorded as a deviation rather than treated as plan-approved work.

### Implement

Implementation is recorded at `9062cb92302c58234e50d34899defc48987c5142`. The development BFF now stores revocable server-side cookie tickets, rejects unverified or unmapped OIDC identities, provisions only explicitly configured local test users, and bounds the development session lifetime. The Aspire + Keycloak + Vite browser journey passed, including same-origin mutation, foreign-Origin and CSRF rejection, logout replay rejection, switching users, and expiry. A PR workflow now runs that journey against disposable local resources and retains browser evidence; it does not use Lighthouse workers.

Local verification passed: 864 backend tests (3 skipped), web build, 62 web unit tests, web lint, changed-file C# formatting, markdownlint, arc42 frontmatter, ProductShape validation and integration checks, Lighthouse verification, and OpenSpec citation verification. The full-solution `dotnet format` check reports pre-existing whitespace violations in untouched source files. GitHub CI is pending publication of this implementation commit, so the FF run remains open.

### Independent audit correction round 1

The independent implementation audit requested stronger coverage for invalid OIDC callbacks and PKCE, real antiforgery form tokens, browser-visible token exposure, and planned visual checks. Commit `7dcbf760d25584fa31e43150ba0b3bd1ea2653de` adds those checks, resolves system theme changes and reduced motion, and records narrow viewport and keyboard focus evidence. The Lighthouse-managed design projection remains generated; DomusMind's own design index states which existing product guidance controls the interface. The full backend suite passed (866 passed, 3 skipped), the web suite/build/lint passed, and the expanded real Aspire browser journey passed locally. Corrected-head GitHub CI and a fresh independent audit remain pending.

### Independent audit correction round 2

The fresh audit of `e3d37dfb003a3160eb19289d4d3d59c0ce312d9d` found that the IndexedDB scan returned an unresolved Promise, so JSON serialization omitted IndexedDB records. Commit `39ea5e435b9e4bc5785959e8f74b5f6733551d42` awaits database enumeration and all object-store reads inside the browser evaluation. It also handles response bodies canceled by navigation while continuing to inspect readable same-origin JSON responses. The clean Aspire + Keycloak + Vite BFF journey passed (1/1, 43.4s); web unit tests passed (62), lint passed (44 existing warnings, no errors), and build passed. CI is running on the corrected head; a fresh independent audit is underway.

The preceding `e3d37df` CI attempt exposed two failures: a Playwright response-body race in the BFF test, now addressed, and the existing household chat journey expected two assistant messages but received one. The latter is outside this code change and must be checked again on `39ea5e4` before Done evidence is complete. The issue's acceptance criteria already cover token storage and session boundaries, so its body did not need another edit; the PR description now points to the corrected head and current evidence.

### Independent audit correction round 3

The independent review of `39ea5e435b9e4bc5785959e8f74b5f6733551d42` stopped pending exact-head CI and asked that IndexedDB keys be scanned in addition to values. Commit `75157551156bcdd5e13e2567bfb336e471b6197b` adds `getAllKeys()` checks for every object store. On this code, the clean Aspire + Keycloak + Vite BFF journey passed (1/1, 43.9s); web unit tests passed (62), lint passed (44 existing warnings, no errors), and build passed. The workflow's broader browser, backend, image, BFF and CodeQL checks are running on this head. GitHub currently reports the PR behind `main`, which remains an integration gate. A final independent audit is still required.

### Final independent audit

The final reviewer inspected exact head `9848ba1a47d7dde1628f43da47e9a38fa93dfe1a` and found no further concrete security defect. It stopped Done because BFF, backend, web, image and CodeQL checks were pending, the prior general browser failure still needed a clean rerun, and PR #278 was draft and behind `main`. The PR description now uses `Closes #277` per the lifecycle contract. Dedicated Bastion runner/operator isolation acceptance and owner-only Done judgement remain explicit gates.

The final reviewer also caught that the PR description used `Refs #277` rather than the required `Closes #277`; the PR body is now corrected and verified. The run remains `running` until checks pass on the final pushed head; the human owner Done and forge gates remain protected.

### Deterministic Done evidence and stop point

All 11 applicable GitHub workflows passed on `1d92eaa01e9ede187999c2db3481a4e392c7517b`, including both browser journeys, backend/web builds, image/Postgres, architecture docs, Lighthouse guardrails, engineering-site build, and CodeQL C#/JavaScript. Conditional ProductShape snapshot, Astro validation and engineering-site publication jobs were skipped as configured. The independent `gpt-6.1-terra` audit approved deterministic evidence on this exact head and confirmed the approved plan hash and security boundary.

Fast Forward now awaits the owner-only Done judgement and required forge approval. PR #278 remains draft and behind `main`; the base must be refreshed and reviewed before any authorized integration. Dedicated Bastion runner/operator isolation acceptance remains pending before Lighthouse workers can execute. The run did not merge, enable workers, release, or deploy.

### Final rerun after main advanced

During the final check cycle, `main` advanced into this branch and a follow-up documentation commit aligned the Development pilot with the separately approved production OIDC contract in #281. No production authentication behavior was added to this issue. On exact head `83c9265253d12f7314ed2c5e7de8916d57fc51f9`, general browser journeys passed (2m30), as did backend/web builds, image/Postgres, architecture, Lighthouse, engineering-site and CodeQL checks. The BFF journey failed after 5m13 because a captured same-origin JSON `response.text()` did not complete before Playwright's 150-second timeout ([workflow](https://github.com/juangcarmona/domusmind/actions/runs/40650571859), [artifact](https://api.github.com/repos/juangcarmona/domusmind/actions/artifacts/12799725512/zip)).

The configured three correction rounds have been used. This FF run is therefore `failed`; it does not claim current-head Done. A fresh authorized run must narrow the response capture to completed/readable API responses, then rerun the complete deterministic lane and independent audit. The issue acceptance criteria remain current. The PR body records this failure; the PR remains draft and no merge, worker enablement, release, or deployment occurred.
