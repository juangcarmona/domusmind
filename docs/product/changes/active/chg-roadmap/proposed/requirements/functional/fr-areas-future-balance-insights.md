---
id: FR-AREAS-FUTURE-BALANCE-INSIGHTS
type: functional-requirement
title: Responsibility balance, overload detection and owner suggestion
status: draft
derived-from:
- UC-AREAS-REVIEW-OWNERSHIP
verification:
- scenario: A person sees how Area ownership is spread across the people of the household
- scenario: A person asks for a suggested Owner for an unowned Area and the Area stays unowned until someone assigns one
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: "src/backend/DomusMind.Application/Features/Responsibilities/GetResponsibilityBalance; src/backend/DomusMind.Application/Features/Responsibilities/DetectResponsibilityOverload; src/backend/DomusMind.Application/Features/Responsibilities/SuggestResponsibilityOwner; interview: product owner decision Q-0059 (E-0158)"
  confidence: low
  recovered-from: interview
---

## Requirement

Planned (future), low confidence until their intent is described (decided: Q-0059): the product MAY show how Area ownership is balanced across the household's people, flag people who carry more Areas than a threshold, and suggest a candidate Owner for an Area. The application code already has queries for all three (observed: src/backend/DomusMind.Application/Features/Responsibilities, GetResponsibilityBalance, DetectResponsibilityOverload with a default threshold of 3, SuggestResponsibilityOwner). A suggestion never changes ownership by itself (BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE).

## Rationale

Named as future direction only; the intent behind balance, overload and suggestion is not yet described.
