---
id: ACT-MICROSOFT-OUTLOOK
type: actor
title: Microsoft Outlook
status: draft
actor-kind: external-system
provenance:
  source: docs/_legacy/01_system/system-spec.md (External Calendar Ingestion Phase 1); docs/_legacy/06_interfaces/external-calendar-api.md
  confidence: high
  recovered-from: documentation
---

## Purpose

The external calendar provider whose entries a member can bring into their own view of the Agenda (observed: system-spec.md, External Calendar Ingestion Phase 1).

## Goals

- Supplies a member's external calendar entries, read through delegated access that the member granted (observed: system-spec.md).

## Responsibilities

- Serves calendar entries for the horizon the member chose: 30, 90, 180 or 365 days forward (observed: system-spec.md).

## Boundaries

- DomusMind never writes back to it and never turns its entries into native plans (observed: system-spec.md).
- Webhooks and bidirectional sync are out of scope (observed: system-spec.md).
