---
id: LC-EXAMPLE
type: domain-lifecycle
title: Example lifecycle
status: draft
subject: TERM-EXAMPLE
states:
  - id: PENDING
    title: Pending
    initial: true
  - id: FINISHED
    title: Finished
    terminal: true
transitions:
  - id: FINISH
    title: Finish the item
    from: [PENDING]
    to: FINISHED
    trigger: The person finishes the item
    initiated-by: [ACT-EXAMPLE]
    governed-by: [BR-EXAMPLE]
    realized-by: [UC-EXAMPLE]
uses-terms: []
---

<!-- Authoring reference for PDaC v1alpha2. Inspect the current field contract
with prodshape schema domain-lifecycle. Local state and transition IDs belong
to this lifecycle. An operation that is refused does not cover a transition. -->

## Purpose

What changes in this concept's product meaning as it moves between states.

## Invariants

Conditions that hold across the lifecycle.

## State Semantics

What each state permits or prevents, beyond its label.

## Transition Semantics

The meaning and observable consequences of the transitions.

## Boundaries

Which neighboring concepts retain their own lifecycle and rules.
