---
id: ACT-EXTERNAL-CALENDAR-CLIENT
type: actor
title: External Calendar Client
status: draft
actor-kind: external-system
---

## Purpose

Read a member's DomusMind calendar subscription in an external calendar application.

## Goals

- Show the subscribed DomusMind plans alongside the person's other calendar information.
- Refresh the feed periodically without changing DomusMind data.

## Responsibilities

- Request the calendar feed using the subscription URL provided by its owner.
- Display the feed as read-only calendar information.

## Boundaries

- The client receives only the information made available by the subscription.
- It cannot create, edit, or cancel a DomusMind plan through the feed.
- Its refresh schedule and presentation are controlled by the external calendar product.
