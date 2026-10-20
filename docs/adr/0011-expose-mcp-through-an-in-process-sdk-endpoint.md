---
name: expose-mcp-through-an-in-process-sdk-endpoint
description: Expose remote AI integrations through the official MCP SDK inside the existing API host.
status: Accepted
date: 2026-10-20
deciders:
  - Product owner
tags:
  - integration
  - mcp
---

# ADR-0011: Expose MCP through an in-process SDK endpoint

## Context

DomusMind's business-facing HTTP API is REST, with controllers as its transport adapters under [ADR-0005](0005-expose-a-rest-api-through-aspnet-core-controllers.md). Remote AI clients need an interoperable MCP interface, which is a distinct protocol and is not a REST API. The accepted MCP architecture assessment selected hosting inside the existing `DomusMind.Api` process; its deployment decision avoids introducing another service. Authentication and household authorization remain a separate prerequisite before any household capability is exposed.

## Decision

We will host MCP Streamable HTTP through the official .NET MCP SDK at `/mcp` inside the existing `DomusMind.Api` process. Business REST routes remain ASP.NET Core controllers under ADR-0005. The MCP adapter remains a thin protocol boundary and does not access persistence or define domain behavior. Until the separate authorization and capability gates pass, the production server exposes no tools, resources, or prompts and returns an empty tools catalog. Configure the SDK to serve the current protocol statelessly while retaining its initialization-handshake compatibility path for down-level clients.

## Consequences

- **Positive:** MCP clients use a standards-based endpoint without adding a deployable component or changing the established REST/controller convention.
- **Negative:** MCP shares the API process, release and operational lifecycle; protocol middleware and route behavior must be tested alongside the SPA fallback.
- **Neutral:** The `/mcp` endpoint is not authenticated in this foundation slice, but exposes no household data or callable capabilities. OAuth/OIDC resource protection and household authorization must be completed before tools are introduced. Current protocol clients are served statelessly; the SDK's compatibility path for initialization-handshake clients uses session state and needs session affinity when horizontally scaled. Host and forwarded-host validation remain distinct from Origin validation.

## Alternatives considered

- **Expose MCP as REST controller actions or hand-written Minimal APIs:** rejected because these would imitate rather than implement the MCP protocol; the SDK owns protocol endpoint behavior.
- **Run a separate MCP deployment:** rejected because the accepted host assessment selected in-process hosting and no deployment constraint justifies an additional service.

## References

- Related: [ADR-0005](0005-expose-a-rest-api-through-aspnet-core-controllers.md), [architecture assessment #226 and merged PR #267](https://github.com/juangcarmona/domusmind/pull/267), [MCP adapter plan #227](../delivery/plans/issue-227-dotnet-mcp-adapter-foundation.md).
- Implementation: [`DomusMind.Mcp`](../../src/backend/DomusMind.Mcp), [API endpoint composition](../../src/backend/DomusMind.Api/Mcp/ApiEndpointRouteBuilderExtensions.cs).
- Specification: [MCP Streamable HTTP transport](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports#streamable-http), [official .NET SDK](https://github.com/modelcontextprotocol/csharp-sdk).
