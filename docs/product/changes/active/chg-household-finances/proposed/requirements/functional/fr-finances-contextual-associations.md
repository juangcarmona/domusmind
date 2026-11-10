---
id: FR-FINANCES-CONTEXTUAL-ASSOCIATIONS
type: functional-requirement
title: Associate financial records with authorized household context
status: draft
derived-from:
  - UC-FINANCES-MANAGE-COMMITMENTS
  - UC-FINANCES-RECORD-COST
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
verification:
  - scenario: "A same-household Expense may reference an authorized Asset, Member or Area without transferring identity, lifecycle or responsibility ownership."
  - scenario: "A financial record can navigate to supporting Documents only when both Finances and Documents accept a typed association and the member can access each target."
uses-terms:
  - TERM-EXPENSE
  - TERM-FINANCIAL-COMMITMENT
---
## Requirement

Financial Commitments and Expenses MAY reference same-household Members, Areas, Assets and, after an accepted contract, Documents. Each target context retains identity, authorization and lifecycle ownership. A contextual link MUST NOT duplicate the target or grant access beyond the caller's permissions.

## Rationale

Households need to answer what an asset, person or area costs without creating duplicate ownership models or bypassing privacy boundaries.
