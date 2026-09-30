---
id: TERM-EXTERNAL-CALENDAR-CONNECTION
type: domain-term
title: "External Calendar Connection"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Outlook connection"
  - "Connected calendar account"
uses-terms:
  - "TERM-MEMBER"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection, External Calendar Configuration); docs/_legacy/04_contexts/calendar.md (ExternalCalendarConnection, External Calendar Connection Integrity); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; interview: product owner decision Q-0049 (E-0158); interview: product owner decision Q-0048 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A person's delegated, read-only link from their DomusMind identity to one account at an external calendar provider (Phase 1: Microsoft Outlook). It records which person owns it, which provider account it reads, which provider calendars are selected, the sync horizon, whether scheduled refresh is on and at what interval, and the current sync status (observed: legacy calendar.md, ExternalCalendarConnection; calendar spec, Outlook Account Connection).

A person may own zero or more connections, each to exactly one provider account (observed: legacy calendar.md, External Calendar Connection Integrity).

## Distinguish From

- **Plan source**: a connection is not a way to create household plans; it is an integration boundary for importing read-only time data (observed: legacy ubiquitous-language.md, External Calendar Connection).
- **Authentication identity**: connecting a provider account does not sign anyone in to DomusMind (inferred from calendar spec, "connect ... to their DomusMind identity").

## Usage

Managed only by its owner from their settings: connect, choose calendars and horizon, sync now, see status, disconnect (observed: external-calendar-api.md, Endpoint Catalog). The Calendar Sync Scheduler refreshes stale connections in the background (observed: calendar spec, Background Feed Refresh). Its status is one of the connection statuses of the domain code: pending initial sync, healthy, syncing, needs attention, partial failure, failed, authorisation expired, rehydrating and disconnected (observed: ExternalCalendarConnectionStatus.cs; decided: Q-0049). A household manager sees a summary of other people's connections, their status but not their content (decided: Q-0048).
