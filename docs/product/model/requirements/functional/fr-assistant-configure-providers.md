---
id: FR-ASSISTANT-CONFIGURE-PROVIDERS
type: functional-requirement
title: Configure the household assistant provider route
status: draft
derived-from:
  - UC-ASSISTANT-CONFIGURE-PROVIDERS
verification:
  - scenario: A household manager configures a primary and optional secondary or tertiary target and sees the effective order.
  - scenario: An ordinary member can see the data-sharing disclosure but cannot retrieve or change provider credentials.
  - scenario: A target lacking the capability required for an assistant operation is not selected for that operation.
uses-terms:
  - TERM-ASSISTANT-PROVIDER-TARGET
---

## Requirement

The product MUST let a household manager configure an ordered route of one to three Assistant Provider Targets. Each target MUST identify its protocol, endpoint and model and MUST securely handle any required credential. The route MUST disclose target order and whether each endpoint is hosted or local to household members. Saved credentials MUST be replaceable but MUST NOT be retrievable in clear text. Ordinary members MAY inspect the route and its data-sharing implications but MUST NOT manage credentials or target configuration.

## Rationale

Households need control over which configured endpoints process their assistant requests while keeping provider secrets protected (issue #143).
