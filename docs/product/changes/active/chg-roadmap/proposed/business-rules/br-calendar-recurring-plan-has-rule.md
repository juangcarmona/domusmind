---
id: BR-CALENDAR-RECURRING-PLAN-HAS-RULE
type: business-rule
title: "A recurring plan must define its recurrence"
status: draft
applies-to:
  - "UC-CALENDAR-MANAGE-RECURRING-PLANS"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Note 1); docs/_legacy/04_contexts/calendar.md (Schedule Semantics, Schedule invariants); interview: product owner decision Q-0044 (E-0158)"
  confidence: "low"
  recovered-from: "documentation"
---

## Rule

Planned: V2 (decided: Q-0044). A recurring plan must define a recurrence rule, such as "every Tuesday" or "every weekday" (observed: legacy calendar.md, Schedule; calendar spec, Note 1).

## Rationale

Recurrence describes repeated time commitments, keeping fixed-time activities in Calendar rather than Tasks (observed: legacy calendar.md, Schedule Semantics).

## Examples

- Football practice every Tuesday (observed: legacy calendar.md).

## Exceptions

None. Recurring plans are future scope for V2 (decided: Q-0044); no current behaviour creates recurring plans and the domain code has no recurrence (inferred from the absence in src/backend/DomusMind.Domain/Calendar), although the openspec says it "captures the invariant" (observed: calendar spec, Note 1).
