---
id: FR-ROADMAP-CALENDAR-SYNCHRONIZATION
type: functional-requirement
title: "Synchronize calendars beyond read-only import"
status: draft
derived-from:
  - "CON-PRODUCT-PHASED-ROADMAP"
verification:
  - scenario: "A person's plan appears in their external calendar and an external change is reflected in DomusMind without manual copying (inferred)."
uses-terms:
  - "TERM-PLAN"
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "docs/_legacy/09_roadmap/roadmap.md (V3 - Intelligence and Integrations); decision Q-0004; docs/_legacy/01_system/system-spec.md (External Calendar Ingestion - Phase 1)"
  confidence: "low"
  recovered-from: "documentation"
---

## Requirement

Planned: V3. The product MAY offer calendar synchronization beyond the Phase 1 read-only, pull-only Outlook import, such as further providers or two-way sync (observed: docs/_legacy/09_roadmap/roadmap.md, V3 "calendar synchronization"; docs/_legacy/01_system/system-spec.md, External Calendar Ingestion Phase 1 "Webhooks and bidirectional sync are out of scope for Phase 1"). Until then CON-CALENDAR-OUTLOOK-PULL-ONLY binds.

## Rationale

V3 aims to reduce friction further by making capture faster, interpretation smarter and coordination more anticipatory; intelligence comes after clarity and automation must strengthen the model, not bypass it (observed: docs/_legacy/09_roadmap/roadmap.md, V3 Outcome, Rule). See CON-PRODUCT-NO-MAGIC-AUTOMATION.
