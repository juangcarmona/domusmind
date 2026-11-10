---
id: BR-DOCUMENT-EXPIRY-DOES-NOT-CREATE-WORK
type: business-rule
title: A document expiry date does not create a household action
status: draft
uses-terms:
  - TERM-DOCUMENT
---
## Rule

A document's issue or expiry date is descriptive metadata. It does not automatically create a Task, Calendar Plan, Agenda entry, reminder, obligation or notification. An owning business context or household member may explicitly decide that a separate action is needed.

## Rationale

The document itself does not know whether an expiry is actionable, legally relevant, already renewed or applicable to the household. Hidden actions would violate DomusMind's explicit-record principle.

## Examples

- A policy expiry date is visible in Documents search/detail but creates no renewal Task.
- A member may explicitly create a Task or the Assets context may define a separate obligation through its own accepted capability.

## Exceptions

Any future automated renewal workflow requires an explicit product change owned by the context that understands the obligation.
