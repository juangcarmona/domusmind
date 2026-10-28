---
id: BR-CHAT-GROUP-MEMBERSHIP
type: business-rule
title: Group conversations have explicit household membership and administration
status: draft
applies-to:
  - UC-CHAT-MANAGE-GROUP-CONVERSATIONS
  - BC-CHATS
uses-terms:
  - TERM-CHAT-GROUP-CONVERSATION
  - TERM-CHAT-THREAD
  - TERM-HOUSEHOLD
---

## Rule

A group conversation has a named identity and an explicit membership list drawn only from the owning household. Its accepted administrators may maintain membership and group identity according to the decisions recorded for the Product Change. Membership changes take effect on every access path and do not rewrite messages already stored in the group.

## Rationale

Group coordination needs a durable access boundary and a visible membership lifecycle; treating a group as an unbounded household feed would make private conversations and assistant context ambiguous.

## Examples

- A group member can read and post in the group.
- A nonmember cannot discover or retrieve the group through a direct identifier.
- Adding or removing a member changes future access without changing existing message authorship.
- The assistant is addressed as a product participant, not added as a household member.

## Exceptions

The exact creator, administrator, history-on-join and group-retirement policies remain open in CHG-HOUSEHOLD-CONVERSATIONS-001 until the product owner decides them.
