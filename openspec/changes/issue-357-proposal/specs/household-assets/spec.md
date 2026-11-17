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
