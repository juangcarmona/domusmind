---
title: DomusMind Architecture
description: Navigation index for DomusMind's architecture documentation and the sources that own each other kind of truth.
---

# DomusMind Architecture

This directory explains **how** DomusMind is structurally shaped, using the twelve
[arc42](https://arc42.org) sections. It does not define what the product does or why;
that belongs to the product model.

## Where each kind of truth lives

| Truth | Owner |
| --- | --- |
| Product intent: actors, requirements, rules, constraints, quality requirements, domain terms | [`docs/product/model/`](../product/model/) (ProductShape) |
| How the surfaces look and behave on screen | [`docs/design/`](../design/README.md) |
| How the system realises that intent | `docs/architecture/` (this directory) |
| Rationale and consequences of significant decisions | [`docs/adr/`](../adr/README.md) |
| Behaviour specifications per capability | [`openspec/specs/`](../../openspec/specs/) |
| Implementation reality | [`src/`](../../src/), [`tests/`](../../tests/), [`deploy/`](../../deploy/), [`.github/workflows/`](../../.github/workflows/) |

Architecture statements that depend on product intent carry a ProductShape citation,
verified with `npx prodshape citations verify docs/architecture`. Where the
architecture and the code disagree, the code is current and the gap is recorded in
[section 11](11-risks-and-technical-debt/README.md).

The **Product Snapshot** is generated from `docs/product` by the
[`product-snapshot`](../../.github/workflows/product-snapshot.yml) workflow for
review artifacts and by the [Engineering site](../engineering-site.md) publisher
for its browsable GitHub Pages copy. The HTML is generated output, not committed.

## Sections

| # | Section | Answers |
| --- | --- | --- |
| 01 | [Introduction and Goals](01-introduction-and-goals/README.md) | What drives the architecture and which quality goals matter most |
| 02 | [Architecture Constraints](02-constraints/README.md) | What restricts design freedom |
| 03 | [Context and Scope](03-context-and-scope/README.md) | Where the system boundary is and who talks to it |
| 04 | [Solution Strategy](04-solution-strategy/README.md) | The fundamental approaches |
| 05 | [Building Block View](05-building-block-view/README.md) | Static decomposition into projects and modules |
| 06 | [Runtime View](06-runtime-view/README.md) | Important runtime interactions |
| 07 | [Deployment View](07-deployment-view/README.md) | Mapping onto infrastructure |
| 08 | [Crosscutting Concepts](08-crosscutting-concepts/README.md) | Reusable mechanisms and conventions |
| 09 | [Architecture Decisions](09-architecture-decisions/README.md) | Index of the ADRs |
| 10 | [Quality Requirements](10-quality-requirements/README.md) | How quality requirements are realised |
| 11 | [Risks and Technical Debt](11-risks-and-technical-debt/README.md) | Known risks and debt |
| 12 | [Glossary](12-glossary/README.md) | Architecture terminology |
