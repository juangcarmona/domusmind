# Spec — Transfer Responsibility

## Purpose

Transfer primary ownership of a responsibility domain to another member.

This operation makes accountability changes explicit and traceable. :contentReference[oaicite:1]{index=1}

## Context

- Module: Responsibilities
- Aggregate: `ResponsibilityDomain`
- Slice: `transfer-responsibility`
- Command: `TransferResponsibility`

## Inputs

Required:

- `responsibilityDomainId`
- `newPrimaryOwnerId`

Optional:

- `effectiveDate`
- `reason`

## Preconditions

- responsibility domain must exist
- new primary owner must exist in the same family
- command modifies only the `ResponsibilityDomain` aggregate

## State Changes

On success, the previous primary owner is replaced by the new one.

The domain remains active and owned.

## Invariants

- a domain may have only one primary owner
- the new owner must belong to the same family
- ownership transfer must preserve a valid owned state

## Events

Emit:

- `ResponsibilityTransferred`

## Success Result

Return:

- `responsibilityDomainId`
- `newPrimaryOwnerId`
- `status = transferred`

## Failure Cases

- responsibility domain not found
- member not found
- invalid transfer

## Notes

Transfer is explicit domain behavior, not an implicit overwrite.