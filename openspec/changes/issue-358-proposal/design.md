<!-- pdac-scope: cited -->
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-CONCURRENT-OUTCOMES" digest="sha256:eab98e328ad2c2e319b35a31c27f5f4630c147c610d3a2761336ae1ab9317b86" -->

# Design

## Context

The Asset aggregate and household-authorized mutation lock already establish ownership and serialize Asset changes. See proposal.md for motivation and the capability specification for observable behavior.

## Goals / Non-Goals

**Goals:** Preserve one pending cycle and durable, auditable outcomes across authorized operations and retries.

**Non-Goals:** Other calendar anchors, timezone changes, corrections, costs, document grants and UI.

## Decisions

- Represent obligations and occurrences as durable household-owned data; use the existing household mutation lock and transactional writes rather than independent unsynchronized updates. Check archived Asset state under the same lock.
- Use local dates, with explicit last-day calendar anchoring and completion-relative advancement. Avoid server or device timezone date conversions.
- Identify each requested operation and target cycle so an exact retry can return its original result, while competing operations cannot act on the next cycle. A plain state check alone cannot ensure retry safety.

## Risks / Trade-offs

- [Concurrent mutations or lost retries] → Persist operation identity and outcome atomically with the cycle and exercise real PostgreSQL competition.
- [Unbounded future generation] → Generate only the next pending cycle and retain historical outcomes.

## Migration Plan

Add storage with a forward migration; deploy with the application. Rollback requires preserving or exporting obligation history before removing its storage.

## Implementation boundaries

Household Settings accepts an initial `timeZoneId` from a household manager. Existing households retain a null value until explicitly configured; obligation definition refuses to guess a server or device timezone. Dates use `DateOnly`, while due/overdue projection converts the current instant through that configured zone. Changing an established timezone remains outside this slice.
<!-- pdac:cite id="BR-ASSET-RECURRENCE-DUE-STATE" digest="sha256:d57ed70f4117a4372e6f7915d6be847be900821d07eb33c0cedbf0d921a3cf8a" -->

`AssetObligation` owns finite occurrence history and immutable operation receipts. Commands mutate that aggregate only, after reading the household-scoped Asset under the existing PostgreSQL household advisory transaction lock. The same lock serializes Asset archive. A version token prevents a stale tracked obligation from overwriting a concurrent result. Exact retries return their stored original receipt after current authorization, including after later lifecycle transitions; changed payloads using the same operation identity conflict. PostgreSQL timestamp precision is normalized in the first response so a durable replay is identical.

The forward migration adds obligation, occurrence and operation storage plus nullable household timezone configuration. Its rollback refuses to discard stored obligations, history or configured timezones. The product model and approved change bytes are unchanged by implementation.
