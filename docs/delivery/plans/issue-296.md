# Plan: clean development-agent installation from DomusMind (#296)

Issue: [#296](https://github.com/juangcarmona/domusmind/issues/296). Base: `617876cbc123e07f0bf4a1b300df1c9816b7afc7`. Branch: `codex/296-clean-agent-install`.

This plan is delivery history for this cleanup. It supplies no ongoing agent instructions, installed skills, automatic lifecycle or replacement Lighthouse installation.

## Ready assessment

The desired outcome is a clean baseline for a later installation, requested by the repository owner. The issue now records the stakeholder and explicit dependencies: code cleanup has none, while external label/settings/runner retirement is a required pre-merge integration gate after exact-head checks and owner PR review/approval. Observable acceptance criteria cover files, packages, workflows, labels, preserved product behavior and checks. Security and visual preservation expectations are explicit. Performance requirements are N/A because runtime algorithms do not change. Product-scope change is N/A. Material risks and existing product/architecture decisions are named. Test impact is below.

Ready passed; `status:ready` was verified after the update. The Plan gate is pending the independent reviewer. No implementation is authorized by this artifact alone. WIP pickup found only #233 in progress, below the cap of three.

## Governing sources and impact boundary

The current `AGENTS.md` and `docs/engineering-lifecycle.md` govern this execution until their reviewed retirement. Product authority remains `docs/product/model/`, accepted Product Changes, `openspec/specs/`, architecture and accepted ADRs. Relevant implementation guidance is in `docs/devops.md`, `docs/design/surface-system.md`, `docs/architecture/05-building-block-view.md`, `06-runtime-view.md`, `08-crosscutting-concepts.md`, ADR-0011 and the development BFF browser workflow.

The affected slice is repository development tooling, generated prompt surfaces and unused visual aliases. Product AI assistants, MCP, the Inbox interpreter, provider settings, domain code, accepted production OIDC intent, development BFF and #281 remain product functionality. A search for “AI”, “agent” or “MCP” is an inventory aid, never a deletion rule.

## Inventory and disposition

The exact tracked removal candidates, edit candidates, retained workflows and repository setting names are in [inventory.json](../ff/296/d5196268-bd84-467c-9154-32702d7aea84/inventory.json). The initial inventory identifies 230 tracked removal candidates. Review each candidate against these rules before removal and record deviations.

| Surface | Planned disposition and reason |
| --- | --- |
| `AGENTS.md`, `CLAUDE.md`, `.agents/`, `.claude/`, `.github/agents/`, `.github/skills/`, `.github/prompts/`, `.github/instructions/`, `skills-lock.json` | Remove all tracked instruction, skill, command, agent and hook surfaces, including dangling installed skill symlinks. The user explicitly clarified `.agents` and `.claude`; no `.cloud` directory exists. |
| Other editor/tool configuration | Audit tracked and untracked hidden names for Codex, Copilot, Cursor, Windsurf, Gemini, Aider, OpenCode, MCP configuration and alternate instruction filenames. Remove repository development-agent installation artifacts if found. Preserve generic editor settings and product MCP code. The isolated checkout currently contains no extra copies. |
| `lighthouse.json`, `LIGHTHOUSE.md`, `.lighthouse/`, `scripts/lighthouse-guard.mjs`, `.husky/` | Remove installation metadata, instructions, verification and Git hooks that invoke the removed guard. Do not mutate shared Git configuration or global tooling. |
| Root `package.json`, `package-lock.json` | Remove both: their only dependencies are Lighthouse Foundation/UI and their only command is `verify:lighthouse`. Retain every application and renderer package and lockfile. |
| `.github/workflows/lighthouse*`, `.github/workflows/shared/lighthouse-opencode.md`, `.github/aw/` | Remove router, verification workflow, five source workflows, five compiled locks, shared prompt and gh-aw action lock. No worker invocation survives in the default branch. |
| Other Actions workflows | Retain all 14 normal backend/web/public/security/product/docs/release/image/browser workflows listed in the inventory. Keep current job names, required checks, `changes` gates and non-agent schedules. |
| `.product/config.yaml`, `.product/installation.lock.json` | Disable AI projection selection explicitly (`ai: []`) and remove the installation lock whose providers contain only generated prompt files. Keep product root, citation settings, templates, validation and canonical model. Confirm the pinned CLI does not regenerate AI artifacts during validation. |
| `openspec/config.yaml`, `.product/integrations/openspec.json` | Inspect their authoring `context`/`rules` and managed prompt metadata as agent residue. Remove instruction-only fields while preserving the minimal schema/provider/citation integration required by current OpenSpec validation. Keep `.product/integrations/openspec.ci.yml` if it remains useful as a non-agent validation example. Record the resulting exact field changes in the PR. |
| `docs/profiles/` | Remove generated Lighthouse profile/adoption guidance. Any still-needed DomusMind security boundary must already be in its architecture/ADR/BFF docs or be expressed there in original project terms before deleting the duplicate guidance. |
| `docs/design/DESIGN.md`, `docs/design/tokens.css`, app `--lh-*` aliases | Remove installed visual guide, unused forwarding stylesheet and six unused aliases. Keep existing DomusMind light/dark colors, typography, density, spacing, motion, responsive rules, components and surface guidance unchanged. Update the design index. |
| `docs/engineering-lifecycle.md`, old #277 plan/run/review | Remove the obsolete agent lifecycle contract and installation-only delivery artifacts. Keep ordinary build, test, release and security guidance in `docs/devops.md`; retain a concise human contribution/check section there if removing the lifecycle would otherwise lose useful engineering guidance. |
| Current docs pointing to removed instructions | Update indexes, devops, E2E and architecture references to retained project architecture, ADRs and current check instructions. Preserve existing product citations and statements. |
| Accepted ADRs and other historical records | Preserve accepted decisions and historical rationale. Replace live links to deleted instruction sources with links pinned to the base commit where necessary; historical mentions of Lighthouse are inert provenance, not active instructions. Do not rewrite accepted decisions to remove history. |
| This issue's plan and FF record | Retain as inert audit evidence. No executable workflow, prompt, skill or configuration reads them. The scan reports this explicit history allowance separately. |

## External state and integration sequencing

At inventory time repository Actions variables were empty, no workflow was in progress, and runner `21` (`bastion-domusmind-gh-aw`, label `lighthouse-workflow`) was online and idle. Repository secret names are limited to Azure/site deployment settings; retain them all. Values were not requested. This snapshot is evidence, not authority to assume the state is unchanged later.

The code PR is prepared and independently reviewed first. External cleanup is a required pre-merge integration gate: complete checks for the exact PR head and obtain owner PR review/approval, execute and verify the exclusive external retirement, then pass Done and merge. These acceptance criteria must not be deferred until after merge. The planning stage makes no external mutations; implementation prepares the reviewable diff and evidence before requesting owner judgement.

1. Confirm all applicable checks and independent final review cover the exact PR head, and obtain the owner’s PR review/approval. Re-read current runs, workflows, variables, secret names, runner ownership and label catalogue. If a worker or owner job is active, stop before retiring its resources. No external mutation occurs before these prerequisites pass.
2. Before deleting labels or runner bindings, disable only the existing Lighthouse workflow activations, including scheduled/manual router and worker registrations, and remove any repository-exclusive activation variable such as `LIGHTHOUSE_AUTOMATION_ENABLED`. Verify no Lighthouse workflow can route a worker while its source still exists on the pre-merge default branch. Keep normal workflow registrations and all unrelated variables. Confirm no active worker remains.
3. Snapshot label assignments for historical audit, then delete exactly the `removeBeforeMerge` catalogue from the inventory. It covers agent reservations, routing/stage/gate labels, canonical/legacy `status:*` labels and installation sizing/health/impact metadata. Catalogue deletion removes assignments from historical issues by GitHub behavior; preserve the snapshot and do not rewrite issue/PR bodies or comments. Retain ordinary classification, product, priority, dependency and generic issue labels in the explicit retention set. Unknown new labels require classification first.
4. Close #287, #288 and #289 as retired automation failures with a link to this cleanup; #295 is already closed as superseded. Keep #281 open and its product scope intact.
5. Remove the repository registration/binding for the dedicated runner only after confirming it is exclusive to DomusMind and idle. Preserve shared Bastion services, binaries, authentication volumes and credentials. Do not rotate, revoke or read credentials. If unregistering requires a credential action, leave that exact step for the owner and record the blocker; do not claim complete cleanup.
6. Before merge, re-query workflows, variable names, secret names, labels, open issues and runner registrations. Record exact removals and retained configuration, then assess every remaining acceptance criterion and owner judgement. The removal of lifecycle labels is itself audited in the run record and issue comments; do not recreate those labels for a final lifecycle transition. Merge only after this gate passes, then verify the integrated branch removes all worker/router files and close #296 when its completion is confirmed.

The exact proposed label deletion and retention sets are machine-readable in the inventory, rather than broad prefix-based deletion at execution time. `owner-review` is included as an SDLC gate; ordinary `blocked`, `audit`, `app-error`, `error-accepted`, `mutation`, `priority` and product labels are retained because their meaning also applies outside agents.

## Ordered implementation tasks

- [ ] Obtain an independent `gpt-6.1-terra` Plan decision covering the committed plan; record it in the same run directory. Advance to `status:proposed` only on approval, then `status:in-progress` when implementation starts.
- [ ] Recheck the clean branch, target SHA and live automation; use the same worktree and exactly one PR.
- [ ] Remove the audited instruction/skill/prompt surfaces, Lighthouse packages/manifests/hooks and source/compiled workflows together.
- [ ] Disable ProductShape AI projections and remove agent authoring configuration without weakening model or citation verification. Confirm normal CLI commands still work.
- [ ] Remove unused visual aliases and package guidance; prove the DomusMind token declarations and consuming component styles are unchanged.
- [ ] Retire installation-only lifecycle/profile/#277 artifacts and repair active documentation/CI references. Preserve useful engineering checks and accepted ADR rationale.
- [ ] Run the acceptance scans and appropriate existing checks below. Record outputs, commands, exact tested commit and limits.
- [ ] Obtain independent final review against the actual diff and all Done dimensions; correct and rerun affected checks within the maximum three rounds. Prepare the single PR for owner review with explicit integration checklist.
- [ ] Stop for owner PR review/approval. After exact-head checks and approval, perform and verify the external cleanup as a required pre-merge integration gate; pass Done before merge. Publish terminal evidence after confirmed integration. Do not recreate retired lifecycle labels to record completion.

## Verification plan

No speculative framework or new mirror tests are required. Use existing tests and focused invariance checks for the deleted tooling.

- Inventory tracked files, hidden names and symlinks with `git ls-files`, `git status --short --untracked-files=all`, and targeted hidden-file searches. Record absent additional editor copies. Never inspect or delete another worktree's local state.
- Scan the resulting tracked tree for Lighthouse packages, executable paths, guard hooks, `--lh-*`, worker dispatch and development-agent configuration. Every surviving historical match must have an explicit rationale; no active authority or install surface is allowed. Ensure all remaining workflow paths resolve and job contexts remain stable.
- Use the existing `docs-ci.yml` arc42 contract check and markdownlint invocation, dropping only the deleted lifecycle path; run `npx -y @prodshape/cli@0.22.0 validate`, `citations verify --provider openspec`, and `citations verify docs/architecture --format json`. Check canonical product/spec/accepted Product Changes are unchanged. Do not invent a PDaC exemption for this tooling cleanup.
- Run Release backend build/tests, web app build/lint/tests and public site build using commands derived from the retained workflows and package scripts. Preserve BFF and product AI/MCP tests; run the existing development BFF browser workflow/check where the environment permits. A required unavailable check is a recorded FF stop, not an implied pass.
- Confirm the CSS diff removes only the unused six aliases/comment and leaves all DomusMind values and consuming styles intact. Capture narrow/wide browser evidence if any rendering declaration or import chain actually changes; avoid redesign.
- Run existing non-agent renderer/site checks when their inputs change; run syntax/reference checks and `git diff --check`. Inspect CI for the final pushed SHA. Keep CodeQL and dependency/secret checks; report any missing expected security evidence accurately.
- Before merge, after exact-head checks and owner PR review/approval, verify the exact label removals, no active router/worker registration, #287–#289 closure, #295 closure and #281 open state. These require live external evidence and cannot be asserted from the branch diff. After merge, additionally verify the integrated branch has no worker/router files.

## Done dimensions and risks

All eleven current DoD dimensions still apply to this run: demonstrated acceptance; complete planned tasks; adequate existing tests; quality checks; security evidence; architecture conformance; updated docs; unchanged product/spec semantics; linked CI/check evidence; owner review; and documented limits. Independent review may cover routine PR review but does not replace the owner's judgement dimensions. The PR remains unmerged until the owner integrates it.

Deleting lifecycle labels while the default branch can still route agents would recreate or retrigger work; disable and verify all exclusive Lighthouse activations before pre-merge label and runner retirement. Do not defer any required external acceptance criterion past merge. Deleting the current lifecycle must not erase the run's audit trail or waive its gates. Generated ProductShape/OpenSpec prompt removal must not disable canonical validation or regenerate the removed skills. GitHub label deletion affects historical assignments; the pre-deletion snapshot preserves provenance. Dedicated runner retirement must not touch shared infrastructure or act on credentials. Remaining historical strings do not prove an installation remains active, but every active reference must be removed.
