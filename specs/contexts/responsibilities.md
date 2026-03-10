# Context Spec — Responsibilities

## Purpose

Defines the functional scope of the Responsibilities context.

This context makes household accountability explicit.

---

## Responsibilities

- create responsibility domains
- assign primary ownership
- assign secondary ownership
- transfer accountability
- maintain valid ownership structure

---

## Aggregate

- `ResponsibilityDomain`

---

## Owned Concepts

- ResponsibilityDomain
- ResponsibilityAssignment

---

## Invariants

- a domain belongs to one family
- a domain has at most one primary owner
- secondary owners are unique
- assigned members must belong to the same family

---

## Events Emitted

- `ResponsibilityDomainCreated`
- `PrimaryOwnerAssigned`
- `SecondaryOwnerAssigned`
- `SecondaryOwnerRemoved`
- `ResponsibilityTransferred`

---

## Events Consumed

- `FamilyCreated`
- `MemberAdded`
- `MemberRemoved`

---

## Related Feature Specs

- create-responsibility-domain
- assign-primary-owner
- assign-secondary-owner
- transfer-responsibility