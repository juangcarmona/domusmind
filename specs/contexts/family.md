# Context Spec — Family

## Purpose

Defines the functional scope of the Family context.

Family is the identity backbone of DomusMind.

---

## Responsibilities

- create household identity
- manage members
- manage dependents
- manage pets inside family structure
- manage structural relationships

---

## Aggregate

- `Family`

---

## Owned Concepts

- Family
- Member
- Dependent
- Pet
- Relationship

---

## Invariants

- a member belongs to exactly one family
- member identifiers are unique within a family
- relationships reference existing family members
- only Family may modify household membership structure

---

## Events Emitted

- `FamilyCreated`
- `MemberAdded`
- `MemberRemoved`
- `DependentAdded`
- `DependentRemoved`
- `PetAdded`
- `PetRemoved`
- `RelationshipAssigned`
- `RelationshipRemoved`

---

## Events Consumed

Usually none.

Family is upstream.

---

## Related Feature Specs

- create-family
- add-member
- assign-relationship
- remove-member