---
id: UC-CALENDAR-REVIEW-HOUSEHOLD-CONNECTION-STATUS
type: use-case
title: "Review the status of the household's external calendar connections"
status: draft
primary-actor: "ACT-HOUSEHOLD-MANAGER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-MANAGER"
provenance:
  source: "docs/_legacy/06_interfaces/external-calendar-api.md (Authorization and Scope: manager summary left to a later rule); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnectionStatus.cs; interview: product owner decision Q-0048 (E-0158)"
  confidence: "medium"
  recovered-from: "interview"
---

## Goal

A household manager knows whether other people's external calendars are feeding the household picture correctly, without seeing what those calendars contain (decided: Q-0048).

## Trigger

The manager wants to check the health of the household's external calendar connections.

## Preconditions

- The person is a household manager (decided: Q-0048).

## Main Flow

1. The manager asks for the household's external calendar connections.
2. DomusMind shows a summary of other people's connections with each connection's status (decided: Q-0048; statuses per Q-0049).

## Alternative Flows

None recorded.

## Failure Conditions

- A person who is not a household manager asks for others' connections: denied (observed: external-calendar-api.md, 403).
- The manager asks for the calendars or entries of another person's connection: not shown; the summary carries status, not content (decided: Q-0048).

## Postconditions

- The manager knows each connection's status; no connection was changed.

Which fields besides status the summary carries, and on which surface it appears, are not specified.
