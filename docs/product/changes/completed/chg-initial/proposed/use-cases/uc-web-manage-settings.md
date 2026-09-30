---
id: UC-WEB-MANAGE-SETTINGS
type: use-case
title: "Find and use configuration in Settings"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors:
  - "ACT-MICROSOFT-OUTLOOK"
governed-by:
  - "BR-WEB-SETTINGS-STAYS-CONFIGURATION"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
uses-terms:
  - "TERM-MEMBER-PROFILE"
  - "TERM-HOUSEHOLD-SETTINGS"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/web-app/spec.md (Settings Surface); docs/_legacy/00_product/surfaces/settings.md (Purpose, Entry Points, Role, Information Architecture, Default View, Mobile Behavior); docs/_legacy/00_product/experience.md (Surface Roles: Settings); interview: product owner decisions Q-0025, Q-0054 (E-0158); interview: product owner review decision (merges)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

The person finds all configuration in one calm place, Settings, without these concerns cluttering the operational surfaces (observed: docs/_legacy/00_product/surfaces/settings.md, Purpose; docs/_legacy/00_product/experience.md, Surface Roles: Settings). This use case covers the Settings surface as a whole: how it is reached, how it is organised and how it behaves. What each section changes, and who may change it, is defined by the use case that owns that outcome and is not restated here.

## Trigger

The person opens Settings from the navigation, the account menu, or a link from the Agenda when calendar setup is missing or sync needs attention (observed: settings.md, Entry Points).

## Preconditions

The person is signed in to their household.

## Main Flow

1. The person opens Settings; it opens on Profile, showing their identity, account details and calendar connections (observed: web-app spec, Settings Surface).
2. From Profile the person edits their own profile (UC-FAMILY-UPDATE-MEMBER-DETAILS) or reviews and manages their Outlook connections (UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS, UC-CALENDAR-CONNECT-OUTLOOK).
3. The person switches to Household, where household settings are configured (UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS) (observed: settings.md, Information Architecture). The Preferences section is reserved and empty in the baseline (decided: Q-0054).
4. DomusMind saves each change in place and shows the result without leaving Settings (inferred).

## Alternative Flows

- On mobile, sections are a stacked list or segmented switch and editing opens a pushed section (observed: settings.md, Mobile Behavior).
- Returning from the Outlook sign-in flow brings the person back to the same section (observed: settings.md, Interaction Rules, Mobile Behavior).

## Failure Conditions

- A person tries to manage another person's calendar connections: not offered in phase 1 (observed: web-app spec, Settings Surface).
- An action the person may not take in a section is rejected as its owning use case states (for household settings, UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS).

## Postconditions

Changes apply where they belong: profile to the person, household settings to everyone, connections to the person's own Agenda (inferred). The Preferences section is reserved and empty until it is defined (decided: Q-0054; settings.md, "personal defaults and future settings seams").
