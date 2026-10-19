---
id: TERM-ASSET-OCCURRENCE
type: domain-term
title: Obligation Occurrence
status: draft
defined-in: BC-HOUSEHOLD-ASSETS
---
## Definition

A bounded historical or current instance of an Asset Obligation's due cycle, retained when the household records an explicit lifecycle action such as completion, skip or reschedule.

## Distinguish From

An occurrence is not an indefinitely pre-created future calendar event or an independently assigned Task.

## Usage

Future due state should be calculated or projected from the obligation without unbounded materialization. Persisted occurrence history must be idempotent and retain its relation to the obligation and any resulting record.
