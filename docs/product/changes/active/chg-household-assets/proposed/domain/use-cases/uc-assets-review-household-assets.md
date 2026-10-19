---
id: UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
type: use-case
title: Review household things, upcoming responsibilities and history
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
uses-terms:
  - TERM-ASSET
  - TERM-ASSET-DOCUMENT
  - TERM-ASSET-RECORD
---
## Goal

Answer what the household manages, where it is, what needs attention, who is responsible and where its information/history can be found.

## Trigger

A member opens the household Assets surface or follows an Agenda obligation projection.

## Preconditions

The member is authorized for the household.

## Main Flow

1. Member searches or filters the compact household asset list.
2. Member sees useful identity, category, location/responsibility and due-attention cues.
3. Member opens the selected Asset inspector to review attributes, obligations, documents/references, records, costs and explicit related work.
4. Member follows an upcoming obligation from Agenda to its Asset and obligation detail.

## Alternative Flows

- A member opens history or documents without any obligation currently due.
- A member follows explicit related Task, Calendar plan or List references to the owning surface.

## Failure Conditions

- A referenced asset or private document is unavailable or the member is unauthorized.

## Postconditions

Member can find the next action and relevant history without treating the surface as a dashboard or task board.
