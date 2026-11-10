---
id: BR-ASSET-OCCURRENCE-HISTORY
type: business-rule
title: Obligation history is bounded, explicit and durable
status: draft
---
## Rule

Do not materialize an unbounded sequence of future occurrences. Calculate or project future due state from the obligation. Retain finite occurrence history when a household member completes, skips or explicitly reschedules a due cycle; record the resulting history once and preserve its relation to the obligation. A completion produces one visible history entry linked to its outcome, not duplicate rows for an occurrence and an Asset Record. If a completion was recorded in error, add a traceable correction that preserves the original outcome rather than overwriting or deleting it. After an Asset is sold or archived, its recorded history remains available read-only.

## Rationale

This avoids unbounded future records while keeping auditable history and recurrence progression.

## Examples

- A recurring annual inspection has one next-due projection and recorded past completions.
- Retrying completion with the same operation does not duplicate the occurrence or Asset Record.
- A mistaken completion stays in the audit trail with a linked correction visible in the same history context.

## Exceptions

The exact persistence model and correction display details remain implementation decisions after product acceptance; usage-meter recurrence is deferred.
