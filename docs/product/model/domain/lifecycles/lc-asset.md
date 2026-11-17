---
id: LC-ASSET
type: domain-lifecycle
title: Asset archive and restore lifecycle
status: active
subject: TERM-ASSET
states:
  - id: ACTIVE
    title: Active
    initial: true
  - id: ARCHIVED
    title: Archived
transitions:
  - id: ARCHIVE
    title: Archive the asset
    from: [ACTIVE]
    to: ARCHIVED
    trigger: An authorized household member removes the Asset through the ordinary action
    initiated-by: [ACT-HOUSEHOLD-MEMBER]
    governed-by: [BR-ASSET-ARCHIVE-AND-RESTORE]
    realized-by: [UC-ASSETS-ARCHIVE]
  - id: RESTORE
    title: Restore the asset
    from: [ARCHIVED]
    to: ACTIVE
    trigger: An authorized household member explicitly restores the Asset
    initiated-by: [ACT-HOUSEHOLD-MEMBER]
    governed-by: [BR-ASSET-ARCHIVE-AND-RESTORE]
    realized-by: [UC-ASSETS-RESTORE]
---

## Purpose

Distinguish a household-managed thing in active use from one put away for consultation that can later return to active use.

## Invariants

The Asset retains one identity and household throughout both states. Archive and restore preserve its stored information, relationships and history. Every operation enforces household/member authorization. Archived Assets are read-only except for explicit restoration. Previously recorded history keeps its existing protection from ordinary editing.

## State Semantics

Active is the initial state after successful registration; ordinary authorized editing and use are available under their governing rules. Archived permits authorized consultation and explicit restoration, while ordinary mutations are refused. Archived is not terminal because it can be restored. This bounded lifecycle has no terminal state; permanent deletion is outside its scope.

## Transition Semantics

Archive changes Active to Archived, with or without existing history, without deleting the Asset or its stored information. Restore changes Archived to Active and makes ordinary editing and use available again for the same Asset. Archive is not a transition from Archived, and restore is not a transition from Active. Attempts outside the declared source state, and unauthorized attempts, make no transition and do not change the Asset. Ordinary edits of an Archived Asset are forbidden and leave it unchanged.

## Boundaries

Registration establishes the initial state; it does not require a fictitious pre-existence state. Definitive deletion, sale or transfer transitions are separate product work. Archive and restore do not silently archive, restore, delete or reassign contained Assets, shared Locations, members, Areas or explicitly linked work. Obligation recurrence and Agenda behavior retain their own rules and require their own lifecycle definition. No transport, status code, storage model or event name is selected here.
