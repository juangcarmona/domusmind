# Cleanup execution evidence for #296

Run: `d5196268-bd84-467c-9154-32702d7aea84`.

Executor: `gpt-6.1-sol`. Independent reviewer: `gpt-6.1-terra`.

The Ready gate passed against the lifecycle at base `617876cbc123e07f0bf4a1b300df1c9816b7afc7`. The issue identifies the stakeholder, dependencies, observable acceptance criteria, quality expectations and risks. One active item (#233) occupies the WIP cap of three. The issue has `enhancement` and `status:ready`.

The plan is proposed in [draft PR #298](https://github.com/juangcarmona/domusmind/pull/298). Independent reviewer `gpt-6.1-terra` requested changes on `15d4fe1b8d0c42bbe71fe239e03614f0f7d166e2`: required external acceptance had been deferred until after merge even though Done must pass first. Correction round 1 reconciles the issue, plan, inventory and PR to make external cleanup a required pre-merge integration gate. A fresh independent Plan decision is pending. No implementation or repository settings were changed. This directory records this cleanup as historical delivery evidence and does not install or authorize an agent workflow. The old lifecycle governs this execution until retirement; its removal does not waive owner review or protected merge gates.

The exact file, workflow and external-setting inventory is in [inventory.json](inventory.json); [run.json](run.json) records stage and review facts. The [plan](../../../plans/issue-296.md) describes the code change and required pre-merge integration actions. Complete checks for the exact PR head and obtain owner PR review/approval first; then verify no worker is active, disable only Lighthouse workflow activations, retire the exact audited labels and idle repository-exclusive runner registration without credential actions, and verify the results before Done and merge. No external mutation was performed for this correction.

Current repository variables: none. The dedicated runner is online and idle. Only unrelated Azure/site secret names were observed; no secret values were read. No new automation run was in progress when inspected. Recheck live state after exact-head checks and owner PR review/approval, before any external cleanup and merge.

Review debt: recent merged #277 and #279 have per-item records; older merged MCP deliveries may have historical review debt, outside this cleanup. No new review debt was created by this plan.

## Plan correction round 2

The reviewer requested changes on `ea1db53b8abcf3e1c44d1d1971ddabe9fc34ecb9` because canonical lifecycle-label deletion conflicted with the old lifecycle's status requirements. The issue, plan and PR now record the owner's explicit one-time authorization to retire all AI/SDLC labels, including canonical/legacy statuses. The old lifecycle governs until its reviewed retirement step. Snapshot all assignments and disable/verify Lighthouse routing before deleting the exact catalogue; do not recreate status labels. The read-only live snapshot has 48 open issues, 45 carrying lifecycle status; recheck before action. Other issue content, retained labels and GitHub states remain unchanged except the authorized failure closures; #281 stays open. Open issues have no lifecycle status until a future separately authorized installation. Close #296 through GitHub issue state after confirmed merge without `status:done`.

No implementation or external configuration mutation was performed. Fresh independent Plan review remains pending.

## Plan approval and implementation

Independent reviewer `gpt-6.1-terra` approved candidate `722fb1029e4b0b781679510576a4479c96ada925` covering head `5e2c0f7ae0575a2a912592f247d77538c83211d0`. Both lifecycle transitions were written and verified: Ready → Proposed → In progress. Implementation started on the same branch and draft PR. External cleanup remains pending exact-head checks and owner PR review/approval; no catalogue, workflow-registration or runner mutation is authorized at this stage.

## Implemented cleanup and verification

Implementation commit: `2c44235e1423d6f9716d612c44cd5771b0e5ca8f`; source candidate after final reference cleanup: `bb7f9bd7bd931a5b267f368f5dd43b2f0430db5a`.

All 230 inventory deletion candidates are gone, including `.agents`, `.claude`, other instruction/skill/prompt surfaces, Lighthouse packages/workflows/configuration/hooks and old installation-only lifecycle material. ProductShape AI projection is explicitly disabled. OpenSpec authoring context/rules, optional schema instruction blocks and embedded refinement prompts were removed while retaining document structures, templates, deterministic scripts and refreshed exact metadata digests. The product CI managed-AI-file checker was retired; native OpenSpec, model and citation checks remain. All 14 normal workflows and original job/check names remain.

Canonical product/specs, backend, tests, public source and BFF behavior are unchanged. Every original DomusMind token declaration is unchanged; only six unused aliases and their comment were removed. Accepted ADR rationale is intact and deleted-source provenance links are pinned to the pre-cleanup revision. The final scan found no active agent reference or new broken link to a deleted file outside explicitly retained history.

The engineering-site architecture renderer remains because the site workflow uses it to publish the public architecture page. Its old references to the removed `architecture-docs` skill were replaced with the engineering-site contract; it is documented as a normal site utility in [cleanup-scan.json](cleanup-scan.json).

Verification passed: Release backend build; 866 backend tests plus the 3 database-backed tests rerun without skips against disposable PostgreSQL; web app build, lint (44 warnings/zero errors) and 62 tests; public site build (35 pages); product model; native OpenSpec; all 823 OpenSpec citations across 15 documents; architecture citations/contract; Markdown over 29 files; renderer/site/report utilities (17 tests); all retained workflow YAML/job contexts; and whitespace checks. The real development BFF browser journey passed 1/1 in 43.8 seconds against disposable Aspire resources, which were stopped afterward. Raw session traces remain local rather than being committed. See [checks.json](checks.json) and [cleanup-scan.json](cleanup-scan.json).

Existing dependency vulnerability warnings remain in NuGet and npm; no product dependency versions changed. These warnings and missing/required hosted security evidence remain subject to owner security judgement. No external workflow registration, label catalogue, repository variable/secret or runner was mutated. Final independent review, exact-head hosted checks, owner review/approval and the pre-merge external retirement gate remain pending; this run is not complete and #296 remains In progress.

## Done audit stopped at the dependency gate

Independent reviewer `gpt-6.1-terra` returned **STOP** for full Done on `c95312a5f55242dfb9605c802eec12ccb9bd1f3c`. The audit found no cleanup defect. All 23 hosted checks for that candidate completed successfully or with their existing expected skips, including CodeQL, product/model/snapshot, architecture, web app, image smoke, browser journeys and Engineering site build; see [ci-c95312a.json](ci-c95312a.json).

The inherited deterministic security gate requires no known vulnerable dependencies. GitHub still reports 15 open Dependabot alerts: 1 critical, 8 high, 3 medium and 3 low. The independent security audit found zero open CodeQL and secret-scanning alerts. Passing CI does not satisfy the unmet dependency gate. See [final-audit.json](final-audit.json).

Run status is **blocked**. Dependency remediation needs separate user direction; after it is completed, rerun the dependency scan and Done review against the resulting candidate. This cleanup does not upgrade unrelated product dependencies. PR #298 remains draft, #296 remains In progress, and external workflow/label/runner retirement and merge remain unperformed. This evidence-only update moves the branch head; its new exact-head hosted checks must also be verified and reported in the PR conversation.
