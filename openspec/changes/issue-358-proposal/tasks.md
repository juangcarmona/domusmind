<!-- pdac-scope: none reason="Implementation checklist; product behavior is cited in the proposal, design and specification." -->

# Tasks

## 1. Domain behavior

- [ ] 1.1 Complete local-date recurrence and obligation/occurrence state transitions; verify all accepted and forbidden states, one-time finish, recurring single-cycle advance, month-end and overdue reactivation with domain tests. Existing candidate coverage is limited to four recurrence-helper tests; lifecycle behavior remains undelivered.

## 2. Durable application boundary

- [ ] 2.1 Add household-authorized persistence, transactional operation identity, API contracts and migration; verify archive guards, history, retries and conflicting concurrent outcomes against PostgreSQL. No obligation persistence, migration, vertical slice or explicit obligation API contract is present in the candidate.
- [ ] 2.2 Persist bounded outcome and reschedule history and enforce stale-cycle and exact-operation retry protection; verify reload and repeated-request behavior against PostgreSQL.
- [ ] 2.3 Serialize obligation mutations with Asset archive and household authorization; verify archived-Asset refusal and completion-versus-skip races using the supplied PostgreSQL service.

## 3. Integration

- [ ] 3.1 Trace both approved lifecycles and all 26 mapped behaviors to actual executable tests and results; explicitly retain uncovered scenarios as gaps.
- [ ] 3.2 Complete the required repository verification and strict implementation-plan check; record terminal receipts and resolve any failures before marking implementation complete.
