# Draft Surface Spec — Documents (working label)

Status: proposed by `CHG-HOUSEHOLD-DOCUMENTS-001`; not accepted product terminology or implementation contract.

## Purpose

Make household paperwork quickly findable, understandable and safely retrievable from one search-first library or the context where it matters. This is a household memory surface, not an enterprise records console or folder-administration product.

## Depends On

- Accepted ProductShape model and `CHG-HOUSEHOLD-DOCUMENTS-001`.
- `docs/design/surface-system.md` and the persistent DomusMind shell.
- Household authorization and the access policy proposed for Documents.
- Context-owned association contracts; Asset links remain contingent on review/alignment of the independent Household Assets Product Change.

## Entry Points

- Main navigation under the final household-facing label.
- Contextual document section/links from Household Member, Asset, Calendar Plan or Task detail where association and access are supported.
- Notifications/search results may link only when the member is currently authorized; no private titles or snippets in unauthorized surfaces.

## Role and Shell

Use the compact page header, central content canvas and contextual inspector. On desktop, selecting a row keeps the result list visible and opens the inspector. Mobile preserves the same content/actions in a pushed detail view or bottom sheet; do not create a separate workflow.

## Default View

Search is prominent and supports title plus approved metadata. The default library is a compact list, not folders or decorative category cards. Rows may show title, type, issuer, relevant date/expiry, linked context and a clear shared/restricted indicator—only for documents the member may discover. Search result counts and filters must not leak restricted records.

Compact filters may include document type, associated member/context, current/expired/archived state and shared/restricted visibility. Full text and sensitive reference numbers are excluded by default. No deep folder hierarchy; optional simple tags remain subject to MVP validation.

## Inspector

The inspector presents:

1. Title, type/category, audience, lifecycle and current file version.
2. Issuer/source, issue and expiry dates, reference number and ordinary tags where permitted.
3. Safe preview for explicitly supported formats, with download as a separate, authorized action; unknown/active formats do not render inline.
4. Associated household member or domain entities, each labeled/navigated through its owner.
5. Version/supersession history, including prior periods where policy allows.
6. Explicit actions: edit metadata, change audience, replace file, add renewed/superseding Document, associate/disassociate, archive/restore, or request deletion as permitted.

Every action requires current authorization. Restricted metadata is not replaced with a revealing placeholder in a surface where its existence itself is private.

## Capture and Interaction

“Add document” supports file selection and deliberate metadata capture with progressive optional fields. The member must choose an explicit audience before saving; do not silently default to all household members. Upload presents progress, validation/scanning state, recoverable failure and retry without exposing a partially accepted file. Validate category and format from product-approved lists.

Replacement must be visibly distinct from renewal: replacement adds a version to the same logical record; renewal creates a new record linked as superseding the old one. Archive and delete remain distinct; confirmations and retention behavior follow the approved policy. Expiry dates are informative and do not silently create reminders or work.

## Contextual Access

Associated surfaces show only documents the current member may discover and retrieve. They display safe metadata and navigation, not copied file content. Access checks apply both to the Document and target context. An Asset integration remains unavailable until the independent Assets Product Change aligns its document ownership and the two contexts' contracts are accepted.

## Accessibility and Responsive Behavior

Use labeled file controls, keyboard-operable search/list/inspector actions, clear focus transitions, screen-reader upload/scan progress, non-color-only shared/restricted/current/expired states, and accessible error/retry messages. At phone widths preserve search/filter, upload and retrieval without horizontal scrolling.

## Open UX Decisions

- Final user-facing label and document category vocabulary.
- Audience selection and default UX, including household-shared vs selected-member access and child-document handling.
- Which formats get inline preview and whether safe preview requires an isolated origin or download-only behavior.
- Revision history visibility, superseding-document cues, archive/trash/delete language and retention feedback.
- Whether external references are supported at all, and how expired or inaccessible external locations are presented.
