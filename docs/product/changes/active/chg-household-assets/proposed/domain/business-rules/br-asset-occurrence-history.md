---
id: BR-ASSET-OCCURRENCE-HISTORY
type: business-rule
title: Obligation history is bounded, explicit and durable
status: draft
---
## Rule

Do not materialize an unbounded sequence of future occurrences. Calculate or project future due state from the obligation. Retain finite occurrence history when a household member completes, skips or explicitly reschedules a due cycle; record the resulting history once and preserve its relation to the obligation.

## Rationale

This avoids unbounded future records while keeping auditable history and recurrence progression.

## Examples

- A recurring annual inspection has one next-due projection and recorded past completions.
- Retrying completion with the same operation does not duplicate the occurrence or Asset Record.

## Exceptions

The exact persistence model and correction policy remain implementation decisions after product acceptance; usage-meter recurrence is deferred.
