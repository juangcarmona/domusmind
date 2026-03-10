# Spec — Create Responsibility Domain

## Purpose

Create a responsibility domain within a family.

A responsibility domain represents an area of household accountability such as food, school, maintenance, or finances. :contentReference[oaicite:0]{index=0}

## Context

- Module: Responsibilities
- Aggregate: `ResponsibilityDomain`
- Slice: `create-responsibility-domain`
- Command: `CreateResponsibilityDomain`

## Inputs

Required:

- `responsibilityDomainId`
- `familyId`
- `name`

Optional:

- `description`

## Preconditions

- target family must exist
- `responsibilityDomainId` must be unique
- `name` must be non-empty
- command must modify a single aggregate boundary :contentReference[oaicite:1]{index=1}

## State Changes

On success, the system creates a new `ResponsibilityDomain` aggregate with:

- stable identifier
- associated `FamilyId`
- domain name
- empty ownership assignments :contentReference[oaicite:2]{index=2}

## Invariants

- a responsibility domain belongs to exactly one family
- domains must have unique identifiers
- ownership assignments may be empty initially :contentReference[oaicite:3]{index=3}

## Events

Emit:

- `ResponsibilityDomainCreated` :contentReference[oaicite:4]{index=4}

## Success Result

Return:

- `responsibilityDomainId`
- `familyId`
- `name`
- `status = created`

## Failure Cases

- family not found
- duplicate `responsibilityDomainId`
- invalid or empty name

## Notes

Ownership may be assigned later through `AssignPrimaryOwner`.