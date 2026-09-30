---
id: SB-CALENDAR-CONNECT-OUTLOOK
type: structured-behaviour
title: "A member connects an Outlook account"
status: draft
illustrates:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
given:
  - "a household member exists"
  - "a successful delegated authorization for their Microsoft account is available"
when: "the member connects the Outlook account"
then:
  - "an external calendar connection is created for that member"
  - "the connection is pending its initial sync"
  - "no plan is created"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection: Member connects an Outlook account)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that connecting prepares ingestion without touching household plans.

## Boundaries

None.
