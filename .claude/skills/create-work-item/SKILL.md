---
name: create-work-item
description: "Create one validated GitHub issue after checking for duplicates. Use when an agreed follow-up or defect needs tracking."
---

One responsibility: create one GitHub issue and return its number and URL.

## Contract

| | |
| --- | --- |
| **Input** | A work type when known, title, description, and required issue fields. |
| **Output** | Issue number and URL. |
| **Writes** | Creates one GitHub issue. |
| **Confirmation** | Required. Show the completed issue and labels, then confirm before creation. |

## Required content

Check the draft against the Definition of Ready in `docs/engineering-lifecycle.md`. Include title, description/context, actor or stakeholder, dependencies (including `None`), acceptance criteria, scope, product rules (or `N/A` with reason), affected behavior (or `N/A`), quality expectations, existing decisions, and unknowns/risks. Keep acceptance criteria under `## Acceptance Criteria`. Mark inapplicable dimensions `N/A` with a short reason. Do not invent missing values; report them and ask the caller.

## Labels

- Work type labels are `bug`, `enhancement`, and `documentation`. Apply at most one, and only when the type is clear.
- Validate every requested label against `gh label list -R juangcarmona/domusmind`. Do not silently drop or invent labels.
- Do not apply a `status:` label during creation. A new issue without one is in Backlog. Classification and lifecycle transition are separate acts.
- Other labels may be applied only when requested and present in the repository.

## How to create

1. Search likely duplicates using the issue title and distinctive terms, for example `gh issue list -R juangcarmona/domusmind --state all --search "<distinctive terms>"`. Inspect likely matches and report them. Do not create a duplicate silently.
2. Validate required content and requested labels. If a required field or requested label is absent/invalid, stop and state what needs correction.
3. Show the full title, body, and exact label list for confirmation. Creation does not transition lifecycle state.
4. After confirmation, create one issue with the approved content and labels, then return the number and URL.

```bash
gh issue create -R juangcarmona/domusmind --title "<title>" --body-file <body-file> --label "<type-label>"
```

Omit `--label` when no type or other label is appropriate. Never chain creation with `transition-work-item`.
