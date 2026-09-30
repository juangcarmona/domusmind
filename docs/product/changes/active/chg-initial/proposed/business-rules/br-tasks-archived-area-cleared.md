---
id: BR-TASKS-ARCHIVED-AREA-CLEARED
type: business-rule
title: Archiving an Area removes it from tasks and routines
status: draft
applies-to:
  - BC-TASKS
  - UC-AREAS-ARCHIVE-AREA
uses-terms:
  - TERM-AREA
  - TERM-TASK
  - TERM-ROUTINE
provenance:
  source: "openspec/specs/tasks/spec.md (Notes 7); interview: product owner decision Q-0010 (E-0158)"
  confidence: medium
  recovered-from: interview
---

## Rule

When an Area is archived, the tasks and routines that reference it MUST lose their Area reference.

## Rationale

The openspec left open what happens to a task's or routine's Area when that Area is archived (observed: openspec/specs/tasks/spec.md, Notes 7); the product owner decided that tasks and routines lose their Area reference (decided: Q-0010).

## Examples

- A task "pay electricity bill" grouped under the Maintenance Area no longer shows an Area once Maintenance is archived; the task itself is otherwise unchanged (inferred example).

## Exceptions

None known. What happens when an Area is deleted permanently is not specified; deleting Areas is not part of the described product (observed: openspec/specs/areas/spec.md, Notes).
