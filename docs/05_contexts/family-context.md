# DomusMind — Family Context

## Purpose

The Family context defines the **household unit** and its internal human structure.

It is responsible for representing:

- the family itself
- its members
- dependents
- pets
- relationships between members

This context establishes the **core identity boundary** of DomusMind.

Most other contexts reference Family, but do not own or modify its internal structure.

---

# Responsibilities

The Family context is responsible for:

- creating a family
- adding and removing members
- registering dependents
- registering pets
- defining relationships between members
- maintaining the household identity boundary

It is the source of truth for **who belongs to a family**.

---

# Aggregate Roots

## Family

The `Family` aggregate is the primary aggregate root of this context.

It owns:

- members
- dependents
- pets
- relationships

All modifications to family structure must occur through `Family`.

---

# Internal Entities

## Member

Represents a person belonging to the family.

Examples:

- adult
- child
- caregiver

## Dependent

Represents a person or dependent entity requiring care within the family.

Examples:

- child
- elderly relative

## Pet

Represents an animal dependent belonging to the family.

## Relationship

Represents a relationship between two members of the same family.

Examples:

- parent-child
- spouse
- sibling
- caregiver-dependent

---

# Value Objects

Suggested value objects:

- `FamilyId`
- `MemberId`
- `DependentId`
- `PetId`
- `RelationshipId`
- `MemberName`
- `PetName`
- `RelationshipType`
- `MemberRole`

The exact value object set may evolve, but identifiers must remain strongly typed.

---

# Invariants

The Family aggregate must enforce the following invariants:

## Identity and Membership

- a family must have a stable `FamilyId`
- a member belongs to exactly one family
- member IDs must be unique within the family
- dependent IDs must be unique within the family
- pet IDs must be unique within the family

## Structural Integrity

- relationships must reference existing members of the same family
- a relationship cannot reference unknown members
- a pet must belong to exactly one family
- a dependent must belong to exactly one family

## Valid State Rules

- a removed member cannot participate in new relationships
- duplicate active members are not allowed
- duplicate active pets are not allowed
- duplicate relationships of the same type between the same active members should not be allowed unless explicitly modeled

## Ownership Boundary

- only the Family context may change family membership structure
- other contexts must reference family members by ID only

---

# Commands

Core commands owned by this context:

- `CreateFamily`
- `AddMember`
- `RemoveMember`
- `AddDependent`
- `RemoveDependent`
- `AddPet`
- `RemovePet`
- `AssignRelationship`
- `RemoveRelationship`

Suggested future commands:

- `RenameFamily`
- `UpdateMemberProfile`
- `ArchiveFamily`

---

# Queries

Core queries supported by this context:

- `GetFamily`
- `GetFamilyMembers`
- `GetFamilyDependents`
- `GetFamilyPets`
- `GetFamilyRelationships`
- `GetMemberById`

Suggested future queries:

- `GetFamilyStructure`
- `GetHouseholdRoster`

---

# Domain Events Emitted

The Family context emits:

- `FamilyCreated`
- `MemberAdded`
- `MemberRemoved`
- `DependentAdded`
- `DependentRemoved`
- `PetAdded`
- `PetRemoved`
- `RelationshipAssigned`
- `RelationshipRemoved`

These events are facts and must be emitted only after successful state change.

---

# Domain Events Consumed

The Family context should consume very few events.

In principle, it should not depend on downstream operational contexts such as:

- Tasks
- Calendar
- Responsibilities
- Food
- Inventory

Possible future consumed events:

- identity import events from external integrations
- household bootstrap events during migration/import flows

Default rule:

**Family is upstream. It emits more than it consumes.**

---

# Read Models

Useful read models for this context:

## Family Summary

Contains:

- family ID
- family name
- total members
- total dependents
- total pets

## Family Roster

Contains:

- all active members
- all dependents
- all pets
- roles
- statuses

## Family Relationship Graph

Contains:

- members
- relationship edges
- relationship types

This read model is especially useful for UI and future AI-assisted reasoning.

---

# Boundaries With Other Contexts

## Responsibilities Context

Responsibilities may be assigned to members defined in Family.

Responsibilities cannot create, modify, or remove members.

Integration rule:

- Responsibilities references `MemberId`
- Family owns membership validity

## Calendar Context

Events may include participants that are members, dependents, or pets.

Calendar does not own those entities.

Integration rule:

- Calendar references participant IDs
- Family owns participant identity

## Tasks Context

Tasks may be assigned to members defined in Family.

Tasks does not own member lifecycle.

## Pets Context

If Pets becomes a separate bounded context in the future, Family should still own **pet membership in the household**, while Pets may own operational pet care details.

For V1, pets remain inside Family.

---

# Ubiquitous Language Notes

Within this context:

- `Family` means the household unit
- `Member` means a person belonging to the household
- `Dependent` means a non-independent household entity requiring care
- `Pet` is a dependent animal belonging to the household
- `Relationship` means a declared structural relationship between members

Do not use ambiguous synonyms such as:

- user
- account member
- profile
- contact
- participant

unless they belong to another explicit context.

---

# Slice Mapping

Initial slices mapped to this context:

- `create-family`
- `add-member`
- `remove-member`
- `add-dependent`
- `add-pet`
- `assign-relationship`

These slices should operate only on the `Family` aggregate.

---

# Transaction Rules

Rules:

- one command modifies one `Family` aggregate
- all structural changes occur inside the `Family` transaction boundary
- downstream reactions occur through domain events
- no cross-aggregate write inside the same command

Example:

`AddMember`
→ updates `Family`
→ emits `MemberAdded`

Then other modules may react asynchronously or after commit.

---

# Design Notes

The Family context is intentionally small and strict.

It should model **identity and household structure**, not operational behavior.

It must not absorb logic that belongs to:

- scheduling
- task execution
- responsibility assignment
- reminders
- food planning
- administration

If the question is “who belongs to this household?”, the answer belongs here.

If the question is “what must happen for this person?”, the answer belongs elsewhere.

---

# Summary

The Family context defines the **identity backbone of DomusMind**.

It owns:

- household identity
- members
- dependents
- pets
- relationships

It is upstream from most other contexts and should remain stable, strict, and small.
