---
name: transition-work-item
description: "Apply exactly one lifecycle state transition to one GitHub Issue by swapping its status label. Use when a stage advances an item."
---

One responsibility: move one work item through exactly one state transition.

## Contract

| | |
| --- | --- |
| **Input** | A GitHub Issue number, and the target lifecycle state. |
| **Output** | The issue's new lifecycle state, and the transition actually applied. |
| **Writes** | The issue's `status:` label in GitHub. |
| **Confirmation** | **Required.** Confirm before applying. |

**Guarantees to the caller**

- The transition contract is read from `docs/engineering-lifecycle.md`; the source state comes from a fresh GitHub read, never an assumed state.
- Exactly one allowed transition is applied per invocation.
- A transition unavailable from the current state is reported together with the transitions allowed by the lifecycle table.
- Pull requests, malformed type/status label sets, and human-only decisions are refused.
- The exact label change is confirmed, the issue is reread before writing, and the resulting state is reread and verified afterward.

**Never**

- Chain several transitions to reach a target state.
- Fire a transition the lifecycle configuration marks human-only.
- Reverse a human-only transition found already applied: report it as a signal instead.

## How to perform

1. Read `docs/engineering-lifecycle.md` and use its stage-to-state table and human-only transition table as the only contract. Do not infer alternate states from GitHub labels.
2. Read the requested record and confirm it is an issue, not a pull request. Use the REST issue representation so the `pull_request` property is visible:

   ```bash
   gh api repos/juangcarmona/domusmind/issues/<number>
   ```

   If `pull_request` is present, stop. Record GitHub's open/closed state separately from lifecycle labels.
3. Validate the full label set. There may be zero or one `status:` label and zero or one work-type label (`bug`, `enhancement`, `documentation`). Multiple labels in either family are invalid; report the conflict and stop. With no `status:` label, the state is Backlog. The requested edge must appear in the lifecycle table, and the target must be reached by this single edge. Never fire Reopen, Block, or Deprioritise.
4. Show the user the issue number/title, GitHub state, current lifecycle state, preserved type label (or none), exact status-label change, and any separate close/open action specified by the lifecycle contract. Ask for confirmation before writing.
5. After confirmation, reread the issue using the same API call. Stop without writing if it became a pull request, its GitHub state changed, its `status:` label changed, or its work-type labels changed. This detects edits made while awaiting confirmation.
6. Apply the one transition in one GitHub operation. Existing lifecycle labels are swapped in the same `gh issue edit` call; Backlog has no source label, so add the first lifecycle label:

   ```bash
   gh issue edit <number> --remove-label "status:<from>" --add-label "status:<to>"
   # For Backlog (no status label):
   gh issue edit <number> --add-label "status:refined"
   ```

7. Reread the issue. Verify the target is the only `status:` label, the work-type labels are unchanged and still at most one, and GitHub open/closed state matches the expected behavior. If the result differs, report the observed state and stop; do not try another transition as a repair.

An open/closed action is separate from label replacement. Only `/integrate` sets `status:done` and closes the issue, as recorded in the lifecycle contract. Do not reopen or close an issue as an incidental effect of another transition.

## Boundary cases

| Observed item | Required behavior |
| --- | --- |
| Backlog with no `status:` label | Treat as Backlog; the only forward edge is `/refine` to `status:refined` |
| `status:in-progress` | Allow only the lifecycle-table edge to `status:in-review`; do not skip a stage |
| Blocked or another unknown `status:` value | Refuse and report it; block is a human decision, not an agent transition |
| Closed issue | Report GitHub `closed` separately; stop if the requested operation would reopen it or contradict `/integrate` behavior |
| Pull request (`pull_request` property present) | Refuse; do not modify labels |
| State or type labels changed after confirmation | Refuse the write and show the newly observed labels/state |
| More than one type or `status:` label | Refuse as malformed; do not guess which label is authoritative |

## Lifecycle states

| State | Label | Meaning |
| --- | --- | --- |
| Backlog | (no `status:` label) | Item exists, not yet refined |
| Refined | `status:refined` | Ready gate passed, ready to plan |
| Planned | `status:planned` | Plan gate passed, ready to implement |
| In Progress | `status:in-progress` | Implementation started |
| In Review | `status:in-review` | Pull request opened |
| Done | `status:done` | Done gate passed, ready to merge |

## Closing

Closing an issue is a separate act from reaching a terminal lifecycle state. The `/integrate` stage sets `status:done` and closes the issue in the same transition, because Done is terminal for this project.

```bash
gh issue close <number>
```

## Transitions no command ever fires

| Transition | Meaning | Who fires it |
| --- | --- | --- |
| Reopen | Reversing Done | human only |
| Block | Item is blocked by a dependency | human only |
| Deprioritise | Item moved back to Backlog from a later state | human only |

The repository is `juangcarmona/domusmind`.
