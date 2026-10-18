---
id: CHG-CALENDAR-SUBSCRIPTIONS
type: product-change
title: Add secure calendar subscriptions
status: proposed
base-revision: 'b37fc3b2999fae400b856b58ed67a42c36f1a093'
operations:
  add:
    - ACT-EXTERNAL-CALENDAR-CLIENT
    - BR-CALENDAR-SUBSCRIPTION-ACCESS
    - FR-CALENDAR-MANAGE-SUBSCRIPTIONS
    - FR-CALENDAR-SERVE-SUBSCRIPTION-FEED
    - QR-CALENDAR-SUBSCRIPTION-SECURITY
    - SB-CALENDAR-CREATE-SECURE-SUBSCRIPTION
    - SB-CALENDAR-READ-SUBSCRIPTION-FEED
    - SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
    - TERM-CALENDAR-SUBSCRIPTION
    - UC-CALENDAR-MANAGE-SUBSCRIPTIONS
    - UC-CALENDAR-READ-SUBSCRIPTION-FEED
  modify:
    - BC-CALENDAR
    - CON-CALENDAR-OUTLOOK-PULL-ONLY
    - CON-PRODUCT-CURRENT-SCOPE
  remove: []
---

## Problem

The accepted Product Definition covers importing read-only Outlook calendar entries into a member's Agenda, but does not describe a member's ability to share DomusMind calendar information with an external calendar client. The current scope also excludes calendar integrations beyond Outlook import.

## Intended Product Outcome

The Calendar definition includes read-only, revocable ICS subscriptions that let an authorized household member share a DomusMind calendar feed with an external calendar client. A subscription is controlled by its owner, and revocation or expiry ends feed access. Existing Outlook connections remain a separate pull-only integration.

## Rationale

Issue [#16](https://github.com/juangcarmona/domusmind/issues/16) describes a read-only calendar feed as a way to show DomusMind plans in calendar clients already used by household members. A private, revocable subscription preserves DomusMind as the source of truth and gives the owner a way to withdraw access.

## Affected Product Areas

- Calendar plans and their read-only sharing outside DomusMind.
- Household member control of a private calendar subscription.
- The current product boundary for calendar integrations.

## Open Questions

- Which household roles may create a subscription: any member with household timeline access, or only a household manager?
- Which plans and other calendar content may a feed expose? In particular, should it include only household plans, member plans, or imported Outlook entries?
- Are visibility scopes required for the first version? The issue lists full-family, member, category, and events-only examples, but does not define their meaning or precedence.
- Which plan fields may be included in a feed? The issue gives title, time, description, and location as examples while also requiring minimal disclosure.
- Is subscription expiry optional or required, and what default duration should apply?

## Product Acceptance

- The accepted definition distinguishes outbound ICS subscriptions from the existing Outlook pull-only connection.
- It identifies who controls a subscription, how an external calendar client accesses its read-only feed, and what revocation guarantees.
- It resolves the open questions about authorization, visible content, available scopes, feed fields, and expiry without weakening household privacy.
- It keeps DomusMind plans authoritative and prevents an external calendar client from changing them.

## Out of Scope

- Writing plans to an external calendar or synchronizing changes back into DomusMind.
- Adding providers for importing external calendars beyond the accepted Outlook connection.
- API, persistence, token-generation, protocol-library, deployment, or implementation design.
- Changes to the Agenda's existing read and edit behavior.
