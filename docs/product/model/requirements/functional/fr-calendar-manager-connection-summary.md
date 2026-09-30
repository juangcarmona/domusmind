---
id: FR-CALENDAR-MANAGER-CONNECTION-SUMMARY
type: functional-requirement
title: "Show a household manager the status of others' connections"
status: draft
derived-from:
  - "UC-CALENDAR-REVIEW-HOUSEHOLD-CONNECTION-STATUS"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
verification:
  - scenario: "Leo's Outlook authorization has expired; Ana, a household manager, sees Leo's connection with its status, and sees none of Leo's calendars or entries (decided: Q-0048)."
  - scenario: "Leo, who is not a manager, asks for Ana's connections and is denied (observed: external-calendar-api.md, 403)."
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-MANAGER"
provenance:
  source: "docs/_legacy/06_interfaces/external-calendar-api.md (Authorization and Scope); interview: product owner decision Q-0048 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Requirement

The product MUST let a household manager see a summary of other people's external calendar connections that shows each connection's status and MUST NOT show their content, such as provider calendars or imported entries (decided: Q-0048). The manager MUST NOT be able to act on another person's connection (observed: external-calendar-api.md, Authorization and Scope).

## Rationale

The manager can notice that part of the household picture is out of date while provider accounts stay personal (decided: Q-0048; inferred for the motivation).
