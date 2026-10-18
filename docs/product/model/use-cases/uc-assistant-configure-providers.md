---
id: UC-ASSISTANT-CONFIGURE-PROVIDERS
type: use-case
title: Configure the household assistant provider route
status: draft
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-CHATS
governed-by:
  - BR-ASSISTANT-PROVIDER-FALLBACK
uses-terms:
  - TERM-ASSISTANT-PROVIDER-TARGET
---

## Goal

Choose and order the providers that may process a household assistant request.

## Trigger

The household manager wants to configure or change how assistant requests are handled.

## Preconditions

- The manager is authenticated and authorized to administer the household.
- The target endpoint supports the product's required text and structured-proposal capabilities.

## Main Flow

1. The manager opens assistant provider settings.
2. DomusMind shows the existing route and the data-sharing boundary of each target.
3. The manager adds, removes, replaces or reorders up to three targets and enters any required credential.
4. DomusMind validates required endpoint details and stores credentials as secrets.
5. DomusMind shows the effective order and explains the data sent to each target.

## Alternative Flows

- The manager can replace a credential without viewing its saved value.
- The manager can configure one target without adding fallbacks.

## Failure Conditions

- A person without manager authority cannot change provider settings.
- Invalid, incomplete or unsupported targets are not added to the route.

## Postconditions

- The household has an ordered route of one to three eligible provider targets.
- Ordinary household members can see the route and its data-sharing disclosure but cannot retrieve credentials.
