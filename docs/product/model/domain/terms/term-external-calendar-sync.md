---
id: TERM-EXTERNAL-CALENDAR-SYNC
type: domain-term
title: "External Calendar Sync"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Synchronization"
  - "Sync now"
  - "Catch-up sync"
  - "Background refresh"
  - "Rehydration"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization, Background Feed Refresh); docs/_legacy/04_contexts/calendar.md (Sync Rule, Recovery Rule); docs/_legacy/06_interfaces/external-calendar-contract-model-catalog.md (Reason values)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

Pulling new, updated and deleted occurrences from each selected feed of a connection, within the sync horizon. It is incremental when valid sync state exists and otherwise a fresh bounded load of the horizon (observed: calendar spec, External Calendar Synchronization).

A sync happens for one of three reasons: **manual** (the person asks), **catch-up** (at sign-in, or when the person opens their Member-scope Agenda, and a connection is stale) and **scheduled** (the background refresh, hourly by default) (observed: calendar spec, Background Feed Refresh; contract-model-catalog, Reason). **Rehydration** is the fresh reload that follows a horizon change or invalid sync state (observed: calendar spec).

## Distinguish From

- **Two-way sync**: DomusMind only pulls; it never writes to the provider and does not receive provider push notifications in Phase 1 (observed: calendar spec, Note 6; legacy system-spec.md).

## Usage

Started by the person (one connection, or all of theirs through "Sync calendars") or by the Calendar Sync Scheduler (observed: external-calendar-api.md, Sync one connection, Sync all connections for one member).
