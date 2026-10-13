---
name: fast-feedback-loop
description: "Run one or more existing work items through DomusMind's refine, plan, implement, review and integrate lifecycle in sequence. Use when the developer asks for an end-to-end delivery loop or a batch of small changes, with explicit plan and merge checkpoints."
---

Run the project's end-to-end delivery loop for the supplied work items. Read `docs/engineering-lifecycle.md` first; its fast feedback loop section defines the gates and authority. This skill coordinates the workflow and does not replace the lifecycle's stage procedures. When a stage skill would normally stop at its gate, return control here only because the developer explicitly requested `/ff`; all gate conditions still apply.

## Input

One or more existing work-item identifiers, in the order to handle them. If none are supplied, ask for them. Do not turn a raw idea into untracked implementation; refine it into a work item first, then resume this loop once it has an identifier.

## Procedure

1. **Establish the starting point.** For each item, read the work item and inspect its lifecycle state, branch and pull request. Resume at the next valid stage; do not duplicate an existing branch, plan or pull request. Report review debt, but do not let unrelated review debt block the item.
2. **Refine.** Check the item against the Definition of Ready. Clarify missing or conflicting requirements and record observable acceptance criteria. If unresolved input is needed, stop on this item and report the question and remaining batch.
3. **Plan.** Inspect the relevant product and architecture guidance, code, callers and tests. Propose a small, ordered implementation plan with test impact, docs impact, risks and Definition of Done evidence. Present the concrete plan and wait for the developer's explicit approval. No implementation before approval.
4. **Implement in feedback slices.** Work through the approved plan in small, coherent steps. After each step, inspect the diff and run focused checks relevant to that step. Keep the plan and task list synchronized. If code, tests or documentation reveal a product-model gap, an accepted decision conflict, or a material deviation, stop and present the finding and proposed next action for approval.
5. **Verify the whole change.** Run the complete applicable checks from the repository's CI and Definition of Done, not only focused checks. Report each command and its observed result. Fix failures and rerun the complete affected check chain. An unavailable check is unmet, not passed.
6. **Review before integration.** Inspect the complete diff against the approved plan and every acceptance criterion. Check for accidental changes, unfinished tasks, missing docs and known limitations. Present the diff summary and verification evidence. The agent does not approve its own work; wait for the developer's explicit approval to merge.
7. **Integrate.** After approval, follow the integration procedure: confirm the Done gate, recheck PR approval and blocking comments, update the branch as required, merge with an explicit message, and verify the merge and post-merge CI result.
8. **Continue the batch.** Only after successful post-merge verification, report the completed item and begin the next supplied item. If any gate fails or needs a decision, stop the batch there and name the remaining items.

## Authority and stop conditions

- The `/ff` request authorizes local investigation, edits, commits and checks needed to carry out the supplied item, subject to repository rules. Existing confirmation requirements for tracker, pull request, push and merge writes still apply. Plan approval and merge approval remain separate, explicit human decisions.
- Never infer approval from silence, a green CI result, an agent review, or the original `/ff` request.
- Do not widen the item's scope or revise accepted product intent silently. Surface the evidence, impact and proposed adjustment, then wait.
- Do not merge with a failed or incomplete Done gate, unresolved blocking review, missing human approval, or failed required CI.
- Do not proceed to another batch item after a blocker, failed check, declined approval or failed post-merge check.
- Report the current state, completed work, check evidence, exact blocker or decision needed, and remaining batch at every stop.
