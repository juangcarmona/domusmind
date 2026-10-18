---
id: CHG-CALENDAR-SUBSCRIPTIONS
type: product-change
title: Add secure calendar subscriptions
status: applied
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

The first version lets any household member who can view the household timeline create a household or single-member subscription. Feeds contain DomusMind Plans only and expose each Plan's title and timing; they omit descriptions, locations, participants and imported Outlook entries. Subscriptions expire after 12 months and are renewed by creating a replacement. Owners can see each subscription's last access time and access count.

## Rationale

Issue [#16](https://github.com/juangcarmona/domusmind/issues/16) describes a read-only calendar feed as a way to show DomusMind plans in calendar clients already used by household members. A private, revocable subscription preserves DomusMind as the source of truth and gives the owner a way to withdraw access.

## Affected Product Areas

- Calendar plans and their read-only sharing outside DomusMind.
- Household member control of a private calendar subscription.
- The current product boundary for calendar integrations.

## Open Questions

None. The approved first-version decisions are: any member with household timeline access may create; scopes are Household or one selected member; feeds contain DomusMind Plans only (not imported Outlook entries); feeds expose title and timing only; all subscriptions expire after 12 months and are renewed by creating a new subscription. Owners can see last access time and access count.

## Product Acceptance

- The accepted definition distinguishes outbound ICS subscriptions from the existing Outlook pull-only connection.
- It identifies who controls a subscription, how an external calendar client accesses its read-only feed, and what revocation guarantees.
- It limits creation to members with household timeline access, offers household and single-member scopes, excludes imported Outlook entries, and exposes only Plan title and timing.
- It requires 12-month expiry, owner-visible last access time and access count, and provides renewal by creating another subscription.
- It keeps DomusMind plans authoritative and prevents an external calendar client from changing them.

## Out of Scope

- Writing plans to an external calendar or synchronizing changes back into DomusMind.
- Adding providers for importing external calendars beyond the accepted Outlook connection.
- API, persistence, token-generation, protocol-library, deployment, or implementation design.
- Changes to the Agenda's existing read and edit behavior.
