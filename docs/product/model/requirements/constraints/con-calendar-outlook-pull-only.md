---
id: CON-CALENDAR-OUTLOOK-PULL-ONLY
type: constraint
title: "Phase 1 external calendars: Outlook only, pull only"
status: draft
applies-to:
  - "BC-CALENDAR"
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose, Note 6); docs/_legacy/04_contexts/calendar.md (External Calendar Integration Boundary); docs/_legacy/01_system/system-spec.md (External Calendar Ingestion Phase 1, Out of Scope for V1)"
  confidence: "high"
  recovered-from: "documentation"
---

## Constraint

In Phase 1 the only external calendar provider is Microsoft Outlook, accessed through the person's delegated authorization. Synchronization is pull-only: no provider push notifications (webhooks), no two-way sync and no write-back to Outlook (observed: calendar spec, Note 6; legacy calendar.md; legacy system-spec.md).

## Rationale

A deliberate Phase 1 scope decision; legacy system-spec.md lists external integrations beyond Outlook ingestion and bidirectional sync as out of scope for V1 (observed). Other providers may be added later without changing plans (observed: legacy calendar.md).

## Consequences

- Freshness depends on manual sync, catch-up and the hourly background refresh; changes in Outlook are not seen instantly (inferred).
- People using Google or other calendars cannot connect them in V1 (inferred).
- Plans created in DomusMind never appear in Outlook (observed: no write-back).
