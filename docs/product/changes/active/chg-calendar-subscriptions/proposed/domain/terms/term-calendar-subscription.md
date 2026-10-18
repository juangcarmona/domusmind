---
id: TERM-CALENDAR-SUBSCRIPTION
type: domain-term
title: Calendar Subscription
status: draft
defined-in: BC-CALENDAR
uses-terms:
  - TERM-HOUSEHOLD
  - TERM-PLAN
---

## Definition

A private, revocable subscription that lets an external calendar client read a calendar feed from DomusMind. The feed is read-only and does not change plans.

## Distinguish From

- **External Calendar Connection**: a member's delegated connection for importing a provider's calendar into DomusMind. A Calendar Subscription shares DomusMind information outward and does not grant DomusMind access to the external account.
- **Plan**: a Calendar Subscription presents selected calendar information; it is not a plan and does not own or change plans.

## Usage

A household member creates a subscription and gives its private URL to an external calendar client. The member can revoke the subscription. Which plans and fields appear in the feed remains subject to the decisions recorded in this Product Change.
