---
id: CON-CALENDAR-OUTLOOK-PULL-ONLY
type: constraint
title: Phase 1 external calendar import is Outlook only and pull only
status: draft
applies-to:
  - BC-CALENDAR
  - UC-CALENDAR-CONNECT-OUTLOOK
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
  - UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
  - TERM-EXTERNAL-CALENDAR-SYNC
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose, Note 6); docs/_legacy/04_contexts/calendar.md (External Calendar Integration Boundary); docs/_legacy/01_system/system-spec.md (External Calendar Ingestion Phase 1, Out of Scope for V1)"
  confidence: high
  recovered-from: documentation
---

## Constraint

For importing external calendar entries in Phase 1, Microsoft Outlook is the only provider. Synchronization from Outlook is pull-only: no provider push notifications (webhooks), no two-way sync and no write-back to Outlook (observed: calendar spec, Note 6; legacy calendar.md; legacy system-spec.md). This constraint governs external calendar import; the outbound DomusMind Calendar Subscription described by CHG-CALENDAR-SUBSCRIPTIONS is separate.

## Rationale

A deliberate Phase 1 scope decision; legacy system-spec.md lists external integrations beyond Outlook ingestion and bidirectional sync as out of scope for V1 (observed). Other providers may be added later without changing plans (observed: legacy calendar.md).

## Consequences

- Freshness of imported Outlook entries depends on manual sync, catch-up and the hourly background refresh; changes in Outlook are not seen instantly (inferred).
- People using Google or other calendars cannot connect them to DomusMind for import in Phase 1 (inferred).
- Plans created in DomusMind are not written back to Outlook (observed: no write-back).
- An outbound Calendar Subscription shares only the calendar information authorized by its owner and does not enable import or write-back (proposed by CHG-CALENDAR-SUBSCRIPTIONS).
