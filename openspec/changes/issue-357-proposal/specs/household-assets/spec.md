<!-- pdac-scope: cited -->

# Household assets

## Purpose

Provide durable household-scoped identities for managed things and their distinct organizational relationships.

<!-- pdac:cite id="FR-ASSETS-REGISTER-AND-ORGANIZE" digest="sha256:6fb7c5fa2cf4f2523b5fd0b829a5b94d6ec3aa35367f8c9aca8adba9bf917ab8" -->

## ADDED Requirements

### Requirement: Household asset identity
An authorized household member SHALL be able to register and find an Active asset with a name, broad category including Other, and optional common details.

#### Scenario: Register an asset
- **WHEN** an authorized member registers a named asset with a category
- **THEN** the asset is Active and belongs to that household

### Requirement: Separate relationships
Assets SHALL support optional containment, reusable locations, management basis, responsible member and Area as independent facts.

#### Scenario: Shared location
- **WHEN** two assets reference the same household location
- **THEN** they share a location without implying containment or responsibility

#### Scenario: Named location
- **WHEN** an authorized member creates a location with a name and no address
- **THEN** the location can be reused by household assets

### Requirement: Scoped references and containment
The system SHALL reject references outside the authorized household and containment that would create a cycle.

#### Scenario: Cross-household reference
- **WHEN** an asset references a member, Area, parent or location outside its household
- **THEN** the change is rejected without modifying the asset

#### Scenario: Cycle
- **WHEN** a parent assignment would make an asset contain itself directly or indirectly
- **THEN** the change is rejected without modifying the asset

### Requirement: Archive and restore
Ordinary removal SHALL archive an Active asset; authorized members SHALL be able to consult it read-only and explicitly restore the same asset to Active.

#### Scenario: Archive and restore
- **WHEN** an authorized member archives and then restores an asset
- **THEN** its identity, information, relationships and history remain unchanged

#### Scenario: Archived asset mutation
- **WHEN** a member attempts an ordinary edit while an asset is Archived
- **THEN** the edit is rejected and the asset remains unchanged

#### Scenario: Invalid or unauthorized transition
- **WHEN** an unauthorized member attempts a transition or a transition is attempted from the wrong state
- **THEN** the asset remains unchanged

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
