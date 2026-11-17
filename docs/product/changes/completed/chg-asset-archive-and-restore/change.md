---
id: CHG-ASSET-ARCHIVE-AND-RESTORE
type: product-change
title: Archive household assets consistently and restore them explicitly
status: applied
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
unaffected:
  - id: BC-HOUSEHOLD-ASSETS
    cause: QR-ASSETS-HOUSEHOLD-PRIVACY
    relationship: applies-to
    digest: sha256:c906b13f8ea4ee4771790626dc32e494b4d4f705885fba1e4939e04359c498b0
    cause-digest: sha256:80c6fbbf71dfbd18480abf113773da8d4a9e053631b5c5a38a8851cd880fe10e
    reason: Asset identity and lifecycle remain in the same context; no ownership boundary changes.
  - id: QR-ASSETS-SEARCH-AND-RESPONSIVENESS
    cause: UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
    relationship: applies-to
    digest: sha256:eb7fc80c4777076d45007165c82317f7085c0f9c2343b5cdfd97d12170ce256c
    cause-digest: sha256:05b5a4c5434549e01871a2fb52e282b19e76ca7841f6388b7c1faaeb3dce66f5
    reason: Consultation still uses the existing search and inspector pattern; no new performance target or layout decision.
  - id: FR-ASSETS-RECORD-HISTORY
    cause: UC-ASSETS-REGISTER-AND-ORGANIZE
    relationship: derived-from
    digest: sha256:9e4e6473c235b06d78248e678734419f9f8202b0e9a35d8c4040a0f2307fd74f
    cause-digest: sha256:47a81de28795ba9eb52e25f350e4d4da9b8227b9cae0947e8098c30348b2b94b
    reason: Archive and restore retain recorded outcomes and the existing restriction on ordinary history editing.
  - id: FR-ASSETS-REFERENCE-DOCUMENTS
    cause: UC-ASSETS-REGISTER-AND-ORGANIZE
    relationship: derived-from
    digest: sha256:bee3f5f7f6e8648f1b8c0af6290a76a57668e720b9ccdb346332cbfcb17bb877
    cause-digest: sha256:47a81de28795ba9eb52e25f350e4d4da9b8227b9cae0947e8098c30348b2b94b
    reason: Retained references keep their document grants and source authorization; neither archive nor restore deletes external content.
  - id: FR-ASSETS-AGENDA-PROJECTION
    cause: UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
    relationship: derived-from
    digest: sha256:5a9498e16e68c6f076a79486ae801c3f1684ec4b46ecaafdf45ea82caf204a79
    cause-digest: sha256:05b5a4c5434549e01871a2fb52e282b19e76ca7841f6388b7c1faaeb3dce66f5
    reason: No new due-state, projection or recurrence behavior is defined here.
  - id: JRN-ASSETS-KEEP-HOUSEHOLD-READY
    cause: UC-ASSETS-REGISTER-AND-ORGANIZE
    relationship: steps[].use-case
    digest: sha256:6fd8373e879872566c89e4f9293259db837f24d9ea291e123eb15cff6472436c
    cause-digest: sha256:47a81de28795ba9eb52e25f350e4d4da9b8227b9cae0947e8098c30348b2b94b
    reason: The existing registration, care and review journey remains valid; archive and restore are explicit lifecycle actions.
  - id: JRN-ASSETS-KEEP-HOUSEHOLD-READY
    cause: UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
    relationship: steps[].use-case
    digest: sha256:6fd8373e879872566c89e4f9293259db837f24d9ea291e123eb15cff6472436c
    cause-digest: sha256:05b5a4c5434549e01871a2fb52e282b19e76ca7841f6388b7c1faaeb3dce66f5
    reason: The existing registration, care and review journey remains valid; archive and restore are explicit lifecycle actions.
  - id: CON-PRODUCT-CURRENT-SCOPE
    cause: TERM-ASSET
    relationship: uses-terms
    digest: sha256:86c4b7c550c3e4edf5c1f614510ece6a13274480c6333c7114e1a103f0219169
    cause-digest: sha256:893ca63bf2743dca5c3cf4e67505c5da80f4dc8afe9e3b19726f2ff6d6ce0669
    reason: Reversible asset archive remains within household-managed things; it adds no inventory or property-management product.
  - id: CON-PRODUCT-HOUSEHOLD-ASSETS-BOUNDARIES
    cause: TERM-ASSET
    relationship: uses-terms
    digest: sha256:cdc3766e60717f042d2a1c5b99f93b4e62ec710bfaa9fa7c4b7fe798d661a659
    cause-digest: sha256:893ca63bf2743dca5c3cf4e67505c5da80f4dc8afe9e3b19726f2ff6d6ce0669
    reason: Cross-context references retain their ownership and authorization boundaries.
  - id: UC-ASSETS-MANAGE-OBLIGATION
    cause: TERM-ASSET
    relationship: uses-terms
    digest: sha256:9db6f4ab6a7706d3cde20a8de2d59f050d933357c2305236d295b1fb1d1683a7
    cause-digest: sha256:893ca63bf2743dca5c3cf4e67505c5da80f4dc8afe9e3b19726f2ff6d6ce0669
    reason: This change does not select an obligation lifecycle or automatic recurrence effects.
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

The owner approved this complete Product Change in PR #368 and authorized its application on 17 November 2026, replying “Si” to “¿Apruebas aplicar este Product Change para retomar #357?”. Apply changes the working baseline; acceptance and delivery remain separate.

## Open Questions

None.

## Product Acceptance

A member can distinguish Active from Archived. Removing an Asset without history and removing one with history both leave the same Asset Archived. An archived Asset can be consulted by an authorized member, ordinary editing is refused, and restoration makes the same Asset Active again without losing its stored information or rewriting history. Unauthorized archive and restore attempts leave the Asset unchanged. The lifecycle and structured behaviours express these outcomes explicitly.

## Out of Scope

Definitive deletion, sale or transfer workflows, new states for obligations or occurrences, recurrence progression or Agenda behavior after archive, document storage, user-facing naming, technical contracts, persistence design, implementation, verification execution, release and deployment. Archive and restore do not silently change the state or ownership of other Assets, Locations, Areas, members or explicitly linked work.
