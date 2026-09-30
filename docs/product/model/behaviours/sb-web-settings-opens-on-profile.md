---
id: SB-WEB-SETTINGS-OPENS-ON-PROFILE
type: structured-behaviour
title: "Settings opens on the person's profile"
status: draft
illustrates:
  - "UC-WEB-MANAGE-SETTINGS"
when: "the person navigates to Settings"
then:
  - "the Profile section is displayed"
  - "the calendar connections section is visible without further navigation"
uses-terms:
  - "TERM-MEMBER-PROFILE"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/web-app/spec.md (Settings Surface: Scenario User opens Settings)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

The most used configuration, a person's own profile and connections, is the default.

## Boundaries

Says nothing about what the profile shows beyond the connections section.
