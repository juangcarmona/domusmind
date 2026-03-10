# Spec — Assign Secondary Owner

## Purpose

Assign a secondary owner to a responsibility domain.

A secondary owner provides backup or shared accountability for the domain. :contentReference[oaicite:0]{index=0}

## Context

- Module: Responsibilities
- Aggregate: `ResponsibilityDomain`
- Slice: `assign-secondary-owner`
- Command: `AssignSecondaryOwner`

## Inputs

Required:

- `responsibilityDomainId`
- `memberId`

Optional:

- `effectiveDate`

## Preconditions

- responsibility domain must exist
- member must exist in the same family
- member must not already be a secondary owner
- command modifies only the `ResponsibilityDomain` aggregate

## State Changes

On success, the member is added to the secondary owner set.

The primary owner remains unchanged.

## Invariants

- secondary owners must be unique within the domain
- assigned members must belong to the same family
- ownership structure must remain valid

## Events

Emit:

- `SecondaryOwnerAssigned`

## Success Result

Return:

- `responsibilityDomainId`
- `memberId`
- `status = assigned`

## Failure Cases

- responsibility domain not found
- member not found
- duplicate secondary owner

## Notes

Secondary owners support redundancy and reduce concentration of mental load.