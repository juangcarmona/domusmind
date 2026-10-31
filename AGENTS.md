# Agent instructions

DomusMind is a domain-centric, API-first modular monolith. Read `docs/README.md` and the relevant product model, OpenSpec, architecture, ADR, and design documents before changing behavior. Product intent changes through a Product Change; accepted ADRs and `docs/design/surface-system.md` govern architecture and visual decisions. Lighthouse profiles are engineering guidance and do not replace those authorities. Map visual profile roles to DomusMind’s existing application tokens; its surface system and palette remain canonical.

Keep ASP.NET Core controllers thin, expose explicit contracts rather than domain entities, use explicit mappings and EF Core directly, and preserve the internal application mediator. Commands affect one aggregate; cross-module collaboration uses persisted domain events. Authentication and member identity remain separate. Keep framework dependencies out of the domain.

Use the existing vertical slices and product terminology. Do not invent missing business rules. Keep changes small and verify the affected paths; `docs/devops.md` lists the repository checks. ProductShape, OpenSpec and ArcShape own their generated integrations: regenerate them through their CLIs rather than editing managed files by hand.

Provider-managed agent integrations may project separate artifacts for Codex and Claude. Preserve their manifests and use the provider's update command to keep those projections current.

Lighthouse's worker workflows require operator provisioning and explicit owner authorization. Do not enable automation, register runners, write provider settings, integrate, merge or deploy without the applicable approval. The worker harness checkout belongs in `.lighthouse-harness/`; DomusMind owns `tools/`.
