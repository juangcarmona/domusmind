---
id: UC-CHAT-MANAGE-GROUP-CONVERSATIONS
type: use-case
title: Household members create and manage group conversations
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-CHATS
governed-by:
  - BR-CHAT-MEMBER-ACCESS
  - BR-CHAT-GROUP-MEMBERSHIP
uses-terms:
  - TERM-CHAT-GROUP-CONVERSATION
  - TERM-CHAT-THREAD
  - TERM-HOUSEHOLD
---

## Goal

Give household members a named, durable group conversation for coordination without turning the household into one undifferentiated message stream.

## Trigger

A household member chooses to create a group or an authorized group administrator changes the group's profile or membership.

## Preconditions

- The acting member belongs to the household.
- The group, if it exists, belongs to the same household.

## Main Flow

1. A member creates a named group with the required profile information.
2. The group is visible only to its authorized members.
3. Authorized group administration adds or removes household members and maintains the group's identity.
4. Members open the group, exchange messages and explicitly address the assistant when needed.

## Alternative Flows

- A member may leave a group when the accepted group lifecycle permits it.
- The product may offer a full-history or join-forward choice when adding a member; the chosen policy must be explicit and recorded.

## Failure Conditions

- A nonmember cannot view, modify or infer the group's content through a direct request.
- A person outside the household cannot be added.
- A membership change that violates the accepted administrator or lifecycle rules is rejected without partial access.

## Postconditions

- The group and its membership persist according to the accepted lifecycle.
- Every group read, write, notification and assistant invocation is evaluated against current membership.
