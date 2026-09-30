---
id: ACT-HOUSEHOLD-MANAGER
type: actor
title: Household Manager
status: draft
actor-kind: human
provenance:
  source: "openspec/specs/family/spec.md (Member manager flag, Member Access Management, Update Member); recovery question Q-0003; interview: product owner decision Q-0022 (E-0158); interview: product owner decision Q-0023 (E-0158); interview: product owner decision Q-0024 (E-0158); interview: product owner decision Q-0025 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Purpose

A household member designated as manager, which grants authority over the household's administrative actions (observed: openspec/specs/family/spec.md).

## Goals

- Keep the household roster and each person's access correct so the shared system reflects who actually lives in the home (inferred from the administrative actions the spec assigns to managers).

## Responsibilities

- Adds people and pets, removes them and manages relationships (decided: Q-0024).
- Changes the household settings (decided: Q-0025).
- Updates other members' core details (name, role and birth date) and any member's role (observed: family spec, Update Member; decided: Q-0023).
- Provisions, disables, enables and resets system access for members (observed: family spec, Member Access Management).
- Does everything a Household Member does.

## Boundaries

- Cannot disable their own access (observed: family spec).
- Must be an adult. The person who creates the household is its first manager; further managers are designated when an adult is added or edited (decided: Q-0022).
