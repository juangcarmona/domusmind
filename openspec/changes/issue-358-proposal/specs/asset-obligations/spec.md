<!-- pdac-scope: cited -->
<!-- pdac:cite id="SB-ASSET-OBLIGATION-INITIAL-PENDING" digest="sha256:e50a2321b6edcfe861a9b5bc7bd3350b2a6d146eef4db8498a080022d326c466" -->
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-CONCURRENT-OUTCOMES" digest="sha256:eab98e328ad2c2e319b35a31c27f5f4630c147c610d3a2761336ae1ab9317b86" -->

# Asset obligations

## Purpose

Provide durable household asset obligations with explicit outcomes and dependable local-date recurrence.

## ADDED Requirements

### Requirement: One pending cycle
The system SHALL create an Active obligation with exactly one Pending cycle and derive due and overdue from household-local dates without inventing missed cycles.

#### Scenario: An unresolved cycle passes its due date
- **WHEN** the local date passes the pending date without an outcome
- **THEN** the same cycle remains Pending and overdue, without generating further cycles.

### Requirement: Recurrence advancement
The system SHALL advance recurring obligations exactly one cycle after completion or skip, preserving the accepted recurrence rule.

#### Scenario: Last-day monthly recurrence
- **WHEN** a January 31 monthly last-day cycle is resolved
- **THEN** the next cycle is due on the last day of February and the last-day anchor remains intact.

#### Scenario: Completion-relative skip
- **WHEN** a completion-relative cycle is skipped
- **THEN** the next date is calculated from the skipped due date rather than the skip date.

#### Scenario: Completion-relative completion
- **WHEN** a completion-relative cycle is completed
- **THEN** the next date is calculated from its actual completion date.

### Requirement: Explicit lifecycle
The system SHALL enforce Active, Suspended, Cancelled and Finished obligation states and preserve terminal occurrence outcomes.

#### Scenario: Suspension and reactivation
- **WHEN** a suspended obligation is explicitly reactivated
- **THEN** the same pending cycle and date remain, including any overdue condition.

#### Scenario: Cancellation
- **WHEN** an Active or Suspended obligation is cancelled
- **THEN** its pending cycle becomes Cancelled, existing history remains, and no completion or skip is invented.

#### Scenario: One-time resolution
- **WHEN** an Active one-time cycle is completed or skipped
- **THEN** the obligation becomes Finished and no new pending cycle is created.

#### Scenario: Forbidden action
- **WHEN** a new complete, skip or reschedule operation targets a Suspended, Cancelled or Finished obligation
- **THEN** the action is refused without changing the obligation or history.

### Requirement: Rescheduling and history
The system SHALL retain a record of date changes and terminal outcomes without changing the recurrence rule or overwriting prior outcomes.

#### Scenario: Reschedule a pending cycle
- **WHEN** an authorized member reschedules an Active pending cycle
- **THEN** the same cycle receives the new date and the original rule remains unchanged.

### Requirement: Authorized atomic operations
The system SHALL restrict mutations to authorized household members on Active Assets and atomically record operation identity and outcome.

#### Scenario: Archived asset
- **WHEN** a member attempts a new obligation mutation on an Archived Asset
- **THEN** the action is refused without changing its obligation, pending date or history.

#### Scenario: Exact retry
- **WHEN** an accepted operation is retried exactly
- **THEN** its prior result is returned without duplicating history or advancing another cycle.

#### Scenario: Competing outcomes
- **WHEN** completion and skip compete for the same cycle
- **THEN** exactly one outcome is accepted and the other cannot resolve a later cycle.
