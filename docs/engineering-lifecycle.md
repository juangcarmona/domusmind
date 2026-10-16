# Engineering lifecycle

**Project**: DomusMind · **Adopted**: 2026-09-08 · **Last amended**: 2026-09-08

## Fast feedback loop

Use `/ff <work-item-id> [work-item-id ...]` to carry one or more existing work items through the lifecycle in sequence. It is an orchestration path over the same `refine`, `plan`, `implement`, `integrate` and `review` gates, not a shortcut around them. A single item can resume from its actual state; a batch advances to the next item only after the current item is merged and its post-merge result is checked.

The loop keeps handoffs short and explicit:

1. **Refine** the problem and acceptance criteria. If the item is not ready, show the missing dimensions and stop for input.
2. **Plan** the solution, small implementation slices, tests and affected documentation. Show the proposed plan and wait for the developer's approval before implementation.
3. **Implement** one verifiable slice at a time. Run focused checks after each slice, then the full applicable Definition of Done checks. Keep the plan aligned with the diff.
4. **Review before merge**: inspect the final diff, acceptance criteria and check evidence. The agent may self-review, but the developer makes the human-review decision and explicitly approves merge. The post-merge `/review` capability is a separate retrospective and remains optional.
5. **Integrate** only after that approval; verify merge status and post-merge CI before advancing to the next supplied item.

The invocation authorizes local investigation and implementation work required by the item. It does not grant approval to change the plan, widen product scope, or merge. Existing confirmation requirements for tracker, pull request, push and merge writes still apply. Stop and surface a proposed adjustment if the product model, an accepted decision, or the approved plan does not cover what implementation requires. Never continue a batch past a failed check, an unresolved blocker, a missing approval, or an unsuccessful merge. Report the completed item, evidence, and the next item or next required decision at each stop.

The individual commands remain available for work that should stop at a single stage. `/ff` is the short path when the developer wants the whole loop handled in one continuous workflow.

## Definition of Ready

Governs the **Ready** gate at the end of the `refine` capability: *is the problem sufficiently defined?* Verified semantically by an agent, which alerts a human when a dimension is missing, vague or contradictory rather than refusing outright.

An item is ready when every applicable dimension is present. Inapplicable ones are marked `N/A` with a one-line reason.

| # | Dimension | This project requires |
| --- | --- | --- |
| 1 | Title | Kept — states the desired outcome, not the activity |
| 2 | Description | Kept — explains the intent, problem and relevant context |
| 3 | Actor / stakeholder | Kept — identifies who needs or is affected by the change |
| 4 | Priority | Dropped — no formal prioritisation; items are picked up by need |
| 5 | Dependencies | Kept — must be explicitly listed, including "none" |
| 6 | Acceptance criteria | Kept — observable and verifiable conditions for success |
| 7 | Scope | Adapted — explicit out-of-scope statement required when ambiguity exists |
| 8 | Product rules | Kept — recorded in `specs/` and `docs/` |
| 9 | Affected behaviour | Kept — when applicable, identify existing flows the change may alter |
| 10 | Quality expectations | Kept — security is relevant (full SAST adopted); performance, accessibility stated when applicable |
| 11 | Test impact | Adapted — identified during Plan, not required at Ready |
| 12 | Existing decisions | Kept — ADRs (`docs/02_architecture/adrs/`), specs (`specs/`), product docs (`docs/`) |
| 13 | Unknowns / risks | Kept — material uncertainty must be recorded, not necessarily resolved |

### Additional to this project

No additional dimensions. The reference dimensions are sufficient for this project.

## GitHub issue classification

GitHub labels carry two independent facts:

| Label family | Allowed values | Meaning |
| --- | --- | --- |
| Work type | `bug`, `enhancement`, `documentation` | What kind of work the issue describes |
| Lifecycle | `status:refined`, `status:planned`, `status:in-progress`, `status:in-review`, `status:done` | Where the issue is in DomusMind's delivery lifecycle |

An issue has at most one work-type label and at most one `status:` label. An issue with no `status:` label is in Backlog. Work type is optional until classification is clear; do not invent a type to satisfy a transition. These labels do not replace GitHub's independent open/closed state, and labels such as `duplicate`, `good first issue`, and `help wanted` remain separate metadata.

When creating an issue, validate the required Ready dimensions below (mark inapplicable dimensions `N/A` with a reason), check for likely duplicates using the title and distinctive terms, and request no more than one existing work-type label. Creation does not advance lifecycle state: omit `status:` so the new issue starts in Backlog, then use the transition workflow as a separate act if a lifecycle transition is valid.

## Definition of Done

Governs the **Done** gate at the end of the `implement` capability: *is the resulting change sufficiently verified?* The `integrate` capability refuses to merge a change that has not passed it.

**In this project, "done" means**: finished and proven, ready to integrate. Deployment is a separate act.

Lane is `deterministic` (an agent or CI settles it), `judgement` (a named role decides), or `both`. Judgement items must name a role from the **Roles** section.

| # | Dimension | This project requires | Lane | Role |
| --- | --- | --- | --- | --- |
| 1 | Acceptance criteria | A test per criterion where feasible; demonstrated in review | both | developer |
| 2 | Implementation completeness | No TODOs, no dead code, no silent scope loss; planned tasks finished or dropped with a recorded reason | both | developer |
| 3 | Tests | xunit + vitest suites green; coverage adequate to the change and the risk it carries | both | developer |
| 4 | Quality checks | `dotnet build`, `dotnet test`, `npm run build`, `eslint`, `tsc` all pass | deterministic | — |
| 5 | Security | Secret scan + dependency scan + SAST (CodeQL) pass; exposure judged by reviewer | both | developer |
| 6 | Architecture / design conformance | Change matches the approach approved at the Plan gate; deviations recorded, not silent | judgement | developer |
| 7 | Documentation | User-facing, operational and developer documentation updated wherever behaviour changed | both | developer |
| 8 | Product / spec consistency | Specs and product docs updated when behaviour changed | both | developer |
| 9 | Evidence | CI run linked; test results exist | deterministic | — |
| 10 | Human review | Self-review against every DoD dimension | judgement | developer |
| 11 | Known limitations | What is deliberately not addressed is stated in the pull request | both | developer |

### Additional to this project

No additional dimensions. The reference dimensions are sufficient for this project.

### Deterministic checks

The commands that produce the deterministic lane's evidence. `verify-like-ci` derives these from the project's pipeline configuration rather than from this list; the list records what the team expects to see, so a silent disappearance is noticed.

| Check | How it runs | Evidence it produces |
| --- | --- | --- |
| Backend build | `dotnet build DomusMind.slnx -c Release` | Build succeeds with no errors |
| Backend tests | `dotnet test DomusMind.slnx -c Release` | All xunit tests pass |
| Web app build | `npm run build` in `src/web/app` | `tsc -b && vite build` succeeds |
| Web app lint | `npm run lint` in `src/web/app` | eslint reports no errors |
| Web app tests | `npm run test` in `src/web/app` | vitest suite passes |
| Public site build | `npm run build` in `src/web/public` | Astro build succeeds |
| Secret scanning | GitHub built-in secret scanning | No secrets detected in diff |
| Dependency scanning | Dependabot alerts | No known vulnerable dependencies |
| SAST | CodeQL workflow | No security alerts |

**Note**: Web app lint, web app tests, secret scanning, dependency scanning, and CodeQL SAST are **expected** checks that are not yet in CI. They are recorded here so their absence is noticed. Adding them to CI is a follow-up task.

## Lifecycle stages and states

How the loop's stages map onto this project's work-item states. The lifecycle state is carried as a `status:<state>` label on each GitHub Issue.

| Stage | State on entry | State on exit | Transition | Fired by |
| --- | --- | --- | --- | --- |
| `/refine` | Backlog (no `status:` label) | Refined | add `status:refined` | agent, human-confirmed |
| `/plan` | Refined | Planned | swap `status:refined` → `status:planned` | `/plan` |
| `/implement` | Planned | In Review | swap `status:planned` → `status:in-progress` on start; `status:in-progress` → `status:in-review` on PR open | `/implement` |
| `/integrate` | In Review | Done | swap `status:in-review` → `status:done`; close issue | `/integrate` |
| `/review` | Done | — | — | — |

The transition skill reads this table before each write. It may apply only one listed transition per invocation, after confirming the exact source and target labels with the user. It rereads the issue immediately before writing and stops if GitHub open/closed state, lifecycle label, or type labels changed since confirmation. After the one-operation label swap it rereads and verifies that the requested lifecycle label is the only `status:` label and that type labels were preserved.

Pull requests are not work items for this lifecycle transition workflow. The issue API exposes an issue-shaped record for a pull request; the skill must inspect the `pull_request` field and refuse it. Reopen, block, and deprioritise remain human-only decisions and are never performed by the transition skill. If a work item is closed, that GitHub state is reported independently from its lifecycle label; `/integrate` keeps the existing behavior of setting `status:done` and closing the issue together.

### Transitions no command ever fires

| Transition | Meaning | Who fires it |
| --- | --- | --- |
| Reopen | Reversing Done back to an earlier state | human only |
| Block | Item is blocked by a dependency or external factor | human only |
| Deprioritise | Item moved back to Backlog from a later state | human only |

## Work item fields

Fields a transition's screen requires. GitHub Issues has no transition screens; the lifecycle state is carried as a `status:<state>` label.

| Field | Identifier | Required by | Notes |
| --- | --- | --- | --- |
| Lifecycle state | `status:<state>` label | every transition | One `status:` label per issue; remove the old label when adding the new one |
| Work type | one of `bug`, `enhancement`, `documentation` | issue creation / classification | Optional; at most one, and independent of lifecycle |
| Acceptance criteria | `## Acceptance Criteria` section in issue body | `/refine` (Ready gate) | Markdown heading in the issue body |
| Work item link | `Closes #<number>` in PR body | `/implement` | Placed in the PR body, not the title |

## Work in progress

**WIP cap**: 3 · **Query used at pickup**: `gh issue list --label "status:in-progress" --state open` + `gh issue list --label "status:in-review" --state open` · **Sprint commitment**: Not used · **Board**: GitHub Issues with `status:` labels. Column limits are not set by any tool; the WIP cap is enforced by the `wip-query` role at pickup.

## Roles

Who decides the judgement items above. A role is a person, a team, or a named responsibility, not a job title for its own sake.

| Role | Who | Decides |
| --- | --- | --- |
| developer | Solo developer (Juan García Carmona) | All Definition of Done judgement dimensions: acceptance criteria, implementation completeness, tests adequacy, security exposure, architecture conformance, documentation, spec consistency, human review, known limitations |

## Installed roles

Role skills maintained in `.claude/skills/`, the single copy that both Claude Code and GitHub Copilot read. Some were installed by `adopt`; `create-work-item` was authored for this repository. Re-running adopt reconciles against this list rather than overwriting blind.

| Role skill | Origin | Added | Location |
| --- | --- | --- | --- |
| `read-work-item` | `references/github-issues.md` | 2026-09-08 | `.claude/skills/` |
| `transition-work-item` | `references/github-issues.md` | 2026-09-08 | `.claude/skills/` |
| `create-work-item` | Authored for GitHub Issues | 2026-10-16 | `.claude/skills/` |
| `open-pull-request` | `references/github.md` | 2026-09-08 | `.claude/skills/` |
| `inspect-ci-result` | `references/github.md` | 2026-09-08 | `.claude/skills/` |
| `merge-pull-request` | `references/github.md` | 2026-09-08 | `.claude/skills/` |
| `wip-query` | `references/github-projects.md` | 2026-09-08 | `.claude/skills/` |

### Roles not installed and why

| Role | Reason |
| --- | --- |
| `commit-to-sprint` | No sprints |
| `create-branch` | Not required; implement creates branches directly |
| `comment-work-item` | Solo developer; progress recorded in PR, not issue comments |
| `update-work-item` | Items updated manually |
| `read-work-item-attachments` | No attachments used |
| `read-work-item-links` | No linking used |
| `read-kb-page` | No specification tool; docs are hand-authored Markdown in-repo |
| `search-kb` | No specification tool |
| `sync-doc-to-kb` | No specification tool |
| `discover-related-docs` | No specification tool |
| `classify-doc-drift` | No specification tool |
| `sync-specs` | No specification tool |
| `propose-change` | No specification tool |
| `apply-change` | No specification tool |
| `archive-change` | No specification tool |
| `inspect-pull-request` | Covered by `open-pull-request` and `merge-pull-request` |
| `update-pull-request` | Covered by `open-pull-request` and `merge-pull-request` |
| `checkout-branch` | Not required; covered by direct git commands |

## Amendment log

The `review` capability proposes changes to this file from delivery evidence. Every amendment is shown as a diff and confirmed by a human; nothing edits this file silently.

| Date | Section | Change | Why |
| --- | --- | --- | --- |
| 2026-09-08 | All | Initial adoption | First run of the `adopt` capability |
| 2026-10-13 | Fast feedback loop | Added `/ff` orchestration with plan, review and merge checkpoints and sequential batch handling | Reduce stage handoffs while preserving lifecycle gates |
| 2026-10-16 | GitHub issue classification and transitions | Defined type labels separately from lifecycle labels; added issue creation guidance and guarded, verified transitions | Make issue classification consistent and lifecycle changes auditable |
