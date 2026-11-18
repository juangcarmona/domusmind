<!-- pdac-scope: cited -->

# Design

## Context

See the proposal and the accepted LC-ASSET and BR-ASSET-HIERARCHY-MEANING. Household membership and Areas are owned by existing modules; the asset context must reference them rather than change them.

<!-- pdac:cite id="FR-ASSETS-REGISTER-AND-ORGANIZE" digest="sha256:6fb7c5fa2cf4f2523b5fd0b829a5b94d6ec3aa35367f8c9aca8adba9bf917ab8" -->

## Goals / Non-Goals

**Goals:** Preserve household isolation, distinct relationships, acyclic containment and non-destructive lifecycle transitions.

**Non-Goals:** UI, permanent deletion, obligations, history creation and changes to accepted product intent.

## Decisions

- Use separate asset and location aggregates, matching their independent identity; a shared location is not containment. Embedding locations in assets would prevent safe reuse.
- Authorize at the application boundary and validate references against the same household before mutation. Unscoped identifier lookups would risk cross-household disclosure.
- Validate parent ancestry transactionally, including concurrent changes; checking only the immediate parent cannot prevent indirect or concurrent cycles.
- Preserve stored data on archive and restore, and reject ordinary edits while archived. Deletion or cascading lifecycle changes would violate the accepted model.
- Follow the existing mediator, explicit contracts and EF Core persistence. Tests span domain invariants and application/API isolation.

## Risks / Trade-offs

- Concurrent reparenting can introduce cycles → serialize competing household hierarchy changes and test the race.
- Historical record creation is not in this slice → preserve existing and future associations without claiming history creation is implemented.

## Migration Plan

Introduce additive tables with scoped indexes and safe rollback only when the new data is no longer needed; rollback must not silently discard household data.

## Implemented contract and consistency

- Routes are household-scoped under `/api/families/{familyId}/assets` and `/asset-locations`. Controllers resolve the authenticated user; a responsible member is a separate optional household reference. Commands and queries use the internal mediator and explicit contracts.
- The category selector returns the stable technical codes `Property`, `Vehicle`, `Appliance`, `Equipment` and `Other`. Management basis remains optional text and never grants ownership or access. Client-facing category wording belongs to the later UI slice.
- Locations are independently registered by name with an optional address, listed and selected for reuse. This slice adds no location removal lifecycle.
- Every asset write acquires a PostgreSQL transaction advisory lock keyed by the household before reading the target or fresh ancestor chain. Domain state and append-only events commit in that transaction. A version concurrency token also rejects stale tracked overwrites. Composite foreign keys constrain parent and location to the same household; member and Area references use scoped validation without changing their owning aggregates.
- Ordinary HTTP DELETE dispatches archive; the explicit archive route uses the same command. Archived assets remain available to authorized queries. No endpoint permanently deletes assets. Archive and restore update only the target aggregate and append their own events.
- The additive migration is generated with `dotnet ef`. Its explicit rollback guard refuses to drop nonempty asset or location tables. Tests exercise both empty rollback/reapply and refusal after data exists.

## Verification

Set `DOMUSMIND_TEST_POSTGRES` to a disposable PostgreSQL connection string and run `npm run verify`. The asset API tests use a test-only authentication handler with the real current-user accessor, family-access service, MVC routing, mediator, handlers, event writer and PostgreSQL persistence. Persistence tests observe the competing backend waiting on a real advisory lock, verify rollback on stale writes, and compare identity, details, references and existing event payloads across archive and restore. Tests isolate their schemas and remove them afterward. When PostgreSQL is absent they explicitly skip; skipped persistence tests are not evidence of qualification.

## Product-derived verification

The PostgreSQL-backed API and persistence tests verify the accepted lifecycle and structured behaviours below. Existing audit entries represent preserved history; this slice does not create Asset Records or obligations.

<!-- pdac:cite id="LC-ASSET" digest="sha256:81cc4efd987b4c4c1403e08c3d72eeb8063388b0bebb5cbe1170a339f68843dc" -->
<!-- pdac:cite id="BR-ASSET-HIERARCHY-MEANING" digest="sha256:4d785768b17ab6fc76fc89eee795c35fee3ea0e6d966b77666b97bd5f9968d77" -->
<!-- pdac:cite id="SB-ASSET-ARCHIVED-CONSULTATION" digest="sha256:7cc4530a1321749e8ecc62dba66736c4f6b208e77f7c30784081781ec4917ba2" -->
<!-- pdac:cite id="SB-ASSET-ARCHIVED-READ-ONLY" digest="sha256:c989b82f947730a882e75770c6af744f184af07b98e7ff5efbdbb41d4302f6a1" -->
<!-- pdac:cite id="SB-ASSET-ARCHIVED-WITH-HISTORY" digest="sha256:87a5a52ff338e8318ee02e723b92e27f566394bae6826d6f884678a160b70181" -->
<!-- pdac:cite id="SB-ASSET-ARCHIVED-WITHOUT-HISTORY" digest="sha256:13e362236f4386a659402d14f1acdeab5067506cb924619a15e35c05b517b8a3" -->
<!-- pdac:cite id="SB-ASSET-REGISTERED-ACTIVE" digest="sha256:7e3e5231a6bd654c764a0012d5f75076710897766eb1c5a8a9d8a76227531f73" -->
<!-- pdac:cite id="SB-ASSET-RESTORED" digest="sha256:188664a26028b0038607d817185de747fee03418886b178831a201cbfd0b3fc7" -->
<!-- pdac:cite id="SB-ASSET-UNAUTHORIZED-ARCHIVE" digest="sha256:1443af4134fbbf621270e8ef51fc0b556b953285efc9004a4af4f1d873cdc2ab" -->
<!-- pdac:cite id="SB-ASSET-UNAUTHORIZED-RESTORE" digest="sha256:058d431a9a590488853e86e66b960200894afe12a49740564a1b4bf992535f8c" -->

| Product scenario | Executable evidence |
| --- | --- |
| Registered Active; archive without records | `HouseholdAssetsApiTests.RequiredDetailsAndCategoryShortlistAreValidated` |
| Archive with existing history; archived consultation; restore | `AssetPersistenceTests.ArchiveRestoreRoundTripPreservesReferencesHistoryAndOtherAssets` and `HouseholdAssetsApiTests.LifecycleUsesRealAuthorizationAndPreservesIdentityDetailsReferencesAndHistory` |
| Archived read-only | Lifecycle API test, domain organization test and stale tracked edit persistence test |
| Unauthorized archive / restore | Lifecycle API test and cross-household API test |
| Separate household relationships and containment | Shared-location API test, cross-household API test and concurrent opposite reparenting persistence test |
