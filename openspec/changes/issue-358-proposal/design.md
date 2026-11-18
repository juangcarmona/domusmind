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
