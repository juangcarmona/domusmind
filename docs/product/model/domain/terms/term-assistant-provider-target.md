---
id: TERM-ASSISTANT-PROVIDER-TARGET
type: domain-term
title: Assistant provider target
status: draft
defined-in: BC-CHATS
synonyms: []
uses-terms: []
---

## Definition

One configured endpoint and model that may receive an assistant request and the authorized household context needed to answer it or prepare an Inbox proposal.

## Distinguish From

- A provider route is the ordered set of configured targets, not one endpoint.
- A household member's authorization determines which context the target may receive; configuring a target grants no data access.

## Usage

A household manager can configure up to three targets in order. A target may be hosted outside the household deployment or run on a local endpoint. DomusMind selects a target only when it supports the capability required for the current operation. Endpoint credentials are secrets and are never shown to ordinary household members or returned after saving.
