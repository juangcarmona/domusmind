---
id: BR-INBOX-REVIEW-AND-EXECUTION
type: business-rule
title: Inbox proposals require an explicit decision
status: draft
applies-to:
  - UC-INBOX-CAPTURE-AND-REVIEW
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Rule

- Capturing or interpreting an Inbox item MUST NOT create or change a Plan, Task, or other household record.
- Each proposal MUST be reviewed and explicitly approved by an authorized household member before its corresponding area behavior is invoked.
- Rejected proposals MUST NOT invoke an area command. Unreviewed proposals MUST remain unapplied.
- Each proposal represents one action and is approved or rejected independently from other proposals for the same Inbox item.
- An approved proposal MUST still pass the owning area's authorization and validation rules.
- The original note remains distinguishable from interpreted fields and from any record created by an approved proposal.

## Rationale

Members need faster capture while remaining responsible for decisions that change the shared household system (issue #17; CON-PRODUCT-NO-MAGIC-AUTOMATION).

## Examples

- A note suggests a Plan and a Task; neither exists until the member approves its proposal.
- The member rejects one proposal and approves another; only the approved action is passed to its owning area.

## Exceptions

None recorded.
