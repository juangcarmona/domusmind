<!-- pdac-scope: none reason="Implementation checklist; product behavior is cited in the proposal, design and specification." -->

# Tasks

## 1. Domain behavior

- [ ] 1.1 Implement local-date recurrence and obligation/occurrence state transitions; verify every accepted and forbidden state and month-end/skip case with domain tests.

## 2. Durable application boundary

- [ ] 2.1 Add household-authorized persistence, transactional operation identity, API contracts and migration; verify archive guards, history, retries and conflicting concurrent outcomes against PostgreSQL.

## 3. Integration

- [ ] 3.1 Run required repository and plan checks and trace the two lifecycles and 26 product scenarios to executed tests.
