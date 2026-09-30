---
id: BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS
type: business-rule
title: "Connecting Outlook requires read and continued access"
status: draft
applies-to:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection); docs/_legacy/04_contexts/calendar.md (Provider and Access Model); docs/_legacy/06_interfaces/external-calendar-api.md (Connect Outlook account rules)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A connection is created only when the person's delegated authorization grants both calendar read access and continued (offline) access; missing either rejects the connection (observed: calendar spec, scopes Calendars.Read and offline_access).

## Rationale

Continued access is what lets background refresh work without the person present (inferred).

## Examples

- An authorization that grants calendar read but not offline access is rejected (observed: calendar spec, missing scopes scenario).

## Exceptions

None.
