# Tasks

<!-- pdac-scope: none reason="Implementation checklist; product behavior is cited in the proposal, design and specification." -->

## 1. Domain foundation

- [x] 1.1 Implement asset and location identity, details and lifecycle; verify domain tests cover transitions and read-only archived assets.

## 2. Persistence and application

- [x] 2.1 Persist assets and locations with an additive migration; verify round-trip and lifecycle preservation tests.
- [x] 2.2 Implement authorized registration, editing, lookup, archive and restore with same-household relationship checks and concurrency-safe cycle rejection; verify isolation and hierarchy tests.

## 3. API and integration

- [x] 3.1 Expose explicit contracts and thin endpoints; verify authorization and lifecycle through API tests.
- [ ] 3.2 Run required repository checks and verify product-derived lifecycle and structured-behaviour coverage.
