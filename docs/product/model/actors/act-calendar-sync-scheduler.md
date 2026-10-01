---
id: ACT-CALENDAR-SYNC-SCHEDULER
type: actor
title: Calendar Sync Scheduler
status: draft
actor-kind: scheduled-process
provenance:
  source: docs/_legacy/01_system/system-spec.md (Sync, pull-based, manual and hourly scheduled refresh); recovery question Q-0003
  confidence: high
  recovered-from: documentation
---

## Purpose

The recurring process that refreshes members' connected external calendars, so imported entries stay current without anyone asking (observed: system-spec.md, "manual and hourly scheduled refresh").

## Goals

- Keeps each member's imported external calendar entries current (inferred from the hourly refresh).

## Responsibilities

- Pulls updated entries from each connected external calendar every hour (observed: system-spec.md).

## Boundaries

- Only reads; it never changes household plans, tasks or lists (observed: system-spec.md, read-only ingestion).
