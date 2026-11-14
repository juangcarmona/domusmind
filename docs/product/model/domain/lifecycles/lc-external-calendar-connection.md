---
id: LC-EXTERNAL-CALENDAR-CONNECTION
type: domain-lifecycle
title: External Calendar Connection lifecycle
status: active
subject: TERM-EXTERNAL-CALENDAR-CONNECTION
states:
  - id: PENDING-INITIAL-SYNC
    title: Pending initial sync
    initial: true
  - id: HEALTHY
    title: Healthy
  - id: SYNCING
    title: Syncing
  - id: PARTIAL-FAILURE
    title: Partial failure
  - id: FAILED
    title: Failed
  - id: AUTH-EXPIRED
    title: Authorization expired
  - id: DISCONNECTED
    title: Disconnected
    terminal: true
transitions:
  - id: START-SYNC
    title: Start a synchronization
    from:
      - PENDING-INITIAL-SYNC
      - HEALTHY
      - SYNCING
      - PARTIAL-FAILURE
      - FAILED
      - AUTH-EXPIRED
    to: SYNCING
    trigger: An authorized sync request acquires the connection lease
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
      - ACT-CALENDAR-SYNC-SCHEDULER
    governed-by:
      - BR-CALENDAR-NO-CONCURRENT-SYNC
      - BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT
    realized-by:
      - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
      - UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS
  - id: SYNC-SUCCESS
    title: Finish a successful synchronization
    from:
      - SYNCING
    to: HEALTHY
    trigger: The sync completes without feed errors
    governed-by:
      - BR-CALENDAR-NO-CONCURRENT-SYNC
      - BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT
    realized-by:
      - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
  - id: SYNC-PARTIAL-FAILURE
    title: Keep successful feeds and report failed feeds
    from:
      - SYNCING
    to: PARTIAL-FAILURE
    trigger: Some selected feeds succeed and others fail
    governed-by:
      - BR-CALENDAR-NO-CONCURRENT-SYNC
      - BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT
    realized-by:
      - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
  - id: SYNC-FAILURE
    title: Report a failed synchronization
    from:
      - SYNCING
    to: FAILED
    trigger: All selected feeds fail or an unexpected sync error occurs
    governed-by:
      - BR-CALENDAR-NO-CONCURRENT-SYNC
      - BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT
    realized-by:
      - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
  - id: AUTH-EXPIRED
    title: Report expired provider authorization
    from:
      - SYNCING
    to: AUTH-EXPIRED
    trigger: The provider access token cannot be refreshed
    governed-by:
      - BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS
    realized-by:
      - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
  - id: DISCONNECT
    title: Disconnect an external account
    from:
      - PENDING-INITIAL-SYNC
      - HEALTHY
      - SYNCING
      - PARTIAL-FAILURE
      - FAILED
      - AUTH-EXPIRED
    to: DISCONNECTED
    trigger: The owner disconnects the connection
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-CALENDAR-DISCONNECT-IS-LOCAL
      - BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS
    realized-by:
      - UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
provenance:
  source: 'TERM-EXTERNAL-CALENDAR-CONNECTION; FR-CALENDAR-SHOW-CONNECTION-STATUS; product owner decision Q-0049; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnectionStatus.cs; src/backend/DomusMind.Application/Features/Calendar/SyncExternalCalendarConnection/SyncExternalCalendarConnectionCommandHandler.cs; product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: medium
  recovered-from: documentation
---

## Purpose

Explain the connection health that a member uses to judge whether imported time information is current.

## Invariants

The connection belongs to its member. Only selected feeds within the horizon are imported. Imports remain read-only, and a connection permits only one running sync. Disconnection affects local imported data and leaves provider records and household Plans unchanged.

## State Semantics

New connections await an initial sync. Healthy records a successful sync; Syncing records work in progress. Partial failure keeps successful feed results and reports failures. Failed reports a failed attempt. Authorization expired means provider access needs attention. Disconnected ends the local connection. The product catalog has seven states: Pending initial sync, Healthy, Syncing, Partial failure, Failed, Authorization expired and Disconnected. A full reload runs within Syncing. Failures show their reason under Failed or Authorization expired.

## Transition Semantics

A manual or scheduled request starts synchronization. Completion records success, partial failure or failure. The domain also records expired authorization as a distinct outcome. Connected, SyncCompleted, SyncFailed and Disconnected events provide evidence for these changes. Configuration changes selected feeds and the horizon without adding another lifecycle state.

## Boundaries

The owner retired Needs attention and Rehydrating as independent product states, superseding that part of Q-0049 (CHG-STATE-MODEL-COMPLETION). The existing domain enum still contains those names; this Product Change does not migrate runtime contracts. The current sync handler records AuthExpired and then its outer exception handler can record Failed; that implementation discrepancy remains an engineering follow-up. This lifecycle expresses the accepted distinction and does not claim that the discrepancy is fixed. Creation establishes the initial state and is not represented as a transition from an imaginary state.
