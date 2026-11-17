---
id: CHG-ASSET-ARCHIVE-AND-RESTORE
type: product-change
title: Archive household assets consistently and restore them explicitly
status: proposed
base-revision: 'ae7dc0b906fabac9cf71dcdabbc6713a8b06c933'
operations:
  add:
    - BR-ASSET-ARCHIVE-AND-RESTORE
    - LC-ASSET
    - UC-ASSETS-ARCHIVE
    - UC-ASSETS-RESTORE
    - SB-ASSET-REGISTERED-ACTIVE
    - SB-ASSET-ARCHIVED-WITHOUT-HISTORY
    - SB-ASSET-ARCHIVED-WITH-HISTORY
    - SB-ASSET-ARCHIVED-READ-ONLY
    - SB-ASSET-ARCHIVED-CONSULTATION
    - SB-ASSET-RESTORED
    - SB-ASSET-UNAUTHORIZED-ARCHIVE
    - SB-ASSET-UNAUTHORIZED-RESTORE
  modify:
    - TERM-ASSET
    - FR-ASSETS-REGISTER-AND-ORGANIZE
    - UC-ASSETS-REGISTER-AND-ORGANIZE
    - UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
    - QR-ASSETS-HOUSEHOLD-PRIVACY
  remove: []
---

## Problem

The accepted definition says ordinary removal archives an Asset with history, but leaves removal without history and subsequent operations on an archived Asset unspecified. It also lacks an explicit Asset lifecycle and examples linking the archive and restore transitions to their observable outcomes.

## Intended Product Outcome

A newly registered Asset is Active. Ordinary removal always archives it, with or without history. An Archived Asset remains available for authorized consultation and is read-only. An explicit restore returns it to Active so ordinary editing and use can resume. Archive and restore preserve identity, household scope, stored information and history. Neither action permanently deletes the Asset.

The lifecycle has Active and Archived states and explicit Archive and Restore transitions. Archived is reversible, so this bounded lifecycle has no terminal state. Definitive deletion and sale-specific transitions remain separate product work.

## Rationale

On 17 November 2026 the product owner chose archive for removal without history and confirmed read-only consultation with explicit restoration. One removal behavior avoids making data retention depend on whether the household has already recorded history. Restoration corrects an unwanted archive without creating a replacement Asset or changing its history.

## Affected Product Areas

Household Assets identity, ordinary removal, authorized consultation, restoration and preservation of stored information and history. Existing household authorization and independent containment, Location and responsibility meanings continue to apply.

The graph also reaches the following accepted artifacts. They were reviewed and retain their existing meaning; listing them here does not add them as unchanged replacement artifacts.

| Existing artifact | Impact assessment |
| --- | --- |
| BC-HOUSEHOLD-ASSETS | Asset identity and lifecycle remain in the same context; no ownership boundary changes. |
| QR-ASSETS-SEARCH-AND-RESPONSIVENESS | Consultation still uses the existing search and inspector pattern; no new performance target or layout decision. |
| FR-ASSETS-RECORD-HISTORY | Archive and restore retain recorded outcomes and the existing restriction on ordinary history editing. |
| FR-ASSETS-REFERENCE-DOCUMENTS | Retained references keep their document grants and source authorization; neither archive nor restore deletes external content. |
| FR-ASSETS-AGENDA-PROJECTION | No new due-state, projection or recurrence behavior is defined here. |
| JRN-ASSETS-KEEP-HOUSEHOLD-READY | The existing registration, care and review journey remains valid; archive and restore are explicit lifecycle actions. |
| CON-PRODUCT-CURRENT-SCOPE | Reversible asset archive remains within household-managed things; it adds no inventory or property-management product. |
| CON-PRODUCT-HOUSEHOLD-ASSETS-BOUNDARIES | Cross-context references retain their ownership and authorization boundaries. |
| UC-ASSETS-MANAGE-OBLIGATION | This change does not select an obligation lifecycle or automatic recurrence effects. |

## Open Questions

None.

## Product Acceptance

A member can distinguish Active from Archived. Removing an Asset without history and removing one with history both leave the same Asset Archived. An archived Asset can be consulted by an authorized member, ordinary editing is refused, and restoration makes the same Asset Active again without losing its stored information or rewriting history. Unauthorized archive and restore attempts leave the Asset unchanged. The lifecycle and structured behaviours express these outcomes explicitly.

## Out of Scope

Definitive deletion, sale or transfer workflows, new states for obligations or occurrences, recurrence progression or Agenda behavior after archive, document storage, user-facing naming, technical contracts, persistence design, implementation, verification execution, release and deployment. Archive and restore do not silently change the state or ownership of other Assets, Locations, Areas, members or explicitly linked work.
