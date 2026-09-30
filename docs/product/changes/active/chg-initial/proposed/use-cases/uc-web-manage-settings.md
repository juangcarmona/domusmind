---
id: UC-WEB-MANAGE-SETTINGS
type: use-case
title: "Manage my profile, household and preferences in Settings"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors:
  - "ACT-MICROSOFT-OUTLOOK"
governed-by:
  - "BR-WEB-SETTINGS-STAYS-CONFIGURATION"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
  - "BR-FAMILY-MANAGERS-ADMINISTER"
uses-terms:
  - "TERM-MEMBER-PROFILE"
  - "TERM-HOUSEHOLD-SETTINGS"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/web-app/spec.md (Settings Surface); docs/_legacy/00_product/surfaces/settings.md (Purpose, Entry Points, Role, Information Architecture, Default View, Mobile Behavior); docs/_legacy/00_product/experience.md (Surface Roles: Settings); interview: product owner decisions Q-0025, Q-0054 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

The person keeps their own details, their calendar connections, the household's preferences and their personal defaults in order, without these concerns cluttering the operational surfaces (observed: docs/_legacy/00_product/surfaces/settings.md, Purpose; docs/_legacy/00_product/experience.md, Surface Roles: Settings).

## Trigger

The person opens Settings from the navigation, the account menu, or a link from the Agenda when calendar setup is missing or sync needs attention (observed: settings.md, Entry Points).

## Preconditions

The person is signed in to their household.

## Main Flow

1. The person opens Settings; it opens on Profile, showing their identity, account details and calendar connections (observed: web-app spec, Settings Surface).
2. The person reviews or changes their profile or their Outlook connections (see the Calendar and Family areas' use cases).
3. The person switches to Household to change household-level preferences (observed: settings.md, Information Architecture). The Preferences section is reserved and empty in the baseline (decided: Q-0054).
4. DomusMind saves each change in place and shows the result without leaving Settings (inferred).

## Alternative Flows

- On mobile, sections are a stacked list or segmented switch and editing opens a pushed section (observed: settings.md, Mobile Behavior).
- Returning from the Outlook sign-in flow brings the person back to the same section (observed: settings.md, Interaction Rules, Mobile Behavior).

## Failure Conditions

- A person tries to manage another person's calendar connections: not offered in phase 1 (observed: web-app spec, Settings Surface).

## Postconditions

Changes apply where they belong: profile to the person, household preferences to everyone, connections to the person's own Agenda (inferred).

The Preferences section is reserved and empty until it is defined (decided: Q-0054; settings.md, "personal defaults and future settings seams"). Only managers may change household settings (decided: Q-0025); every person manages their own profile and calendar connections.
