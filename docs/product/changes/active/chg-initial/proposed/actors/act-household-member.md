---
id: ACT-HOUSEHOLD-MEMBER
type: actor
title: Household Member
status: draft
actor-kind: human
provenance:
  source: "docs/_legacy/00_product/strategy.md (Target Audience, Value Proposition); openspec/specs/family/spec.md (Member, system access); recovery question Q-0003; interview: product owner decision Q-0023 (E-0158); interview: product owner decision Q-0024 (E-0158); interview: product owner decision Q-0025 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Purpose

A person who belongs to the household and has been given access to DomusMind. They use the shared household system so that coordinating the home no longer depends on one person remembering everything (observed: strategy.md, Mission and Value Proposition).

## Goals

- See what is happening, what needs attention, who is responsible and what happens next (observed: strategy.md, Value Proposition).
- Understand what matters today without depending on another person's memory (observed: roadmap.md, V1 success shape).
- Capture plans, tasks and list items with less effort than remembering them (observed: strategy.md, Differentiators).

## Responsibilities

- Schedules plans, creates and completes tasks, keeps shared lists, and reads the Agenda (observed: system-spec.md, Capability Groups).
- Holds ownership of household areas when assigned (observed: system-overview.md, Responsibilities).
- May connect a personal external calendar whose entries appear only in their own Agenda scope (observed: system-spec.md, External Calendar Ingestion).

## Boundaries

- Does not perform household administration such as adding or removing people and pets, managing relationships, changing household settings, editing other members or provisioning their access; that is the Household Manager (observed: openspec/specs/family/spec.md; decided: Q-0024, Q-0025).
- May change their own name and birth date, but not their own role (decided: Q-0023).
- Members with the Pet role never act in the product: they cannot be given system access (observed: openspec/specs/family/spec.md, Pet Registration).
