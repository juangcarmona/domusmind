# Issue #227: .NET MCP adapter foundation

## Outcome

Add a minimal, empty-capability MCP server endpoint to the existing `DomusMind.Api` process. Establish a testable protocol boundary that can later invoke application-owned operations without creating a second deployable, touching persistence from adapter code, or exposing household data before authorization and product gates pass.

## Accepted constraints and repository evidence

- Owner accepted the in-process `DomusMind.Api` host direction and OAuth 2.1 Authorization Code + PKCE/OIDC direction in issue #226. Authorization-server placement/provider remains open for #229 and is not part of this issue.
- DomusMind is a .NET 10 API-first modular monolith. `DomusMind.Api` currently composes Application and Infrastructure; existing REST APIs remain ASP.NET Core Controllers.
- Introduce a non-deployable `DomusMind.Mcp` class library referenced by the API host. The existing `DomusMind.Application` project itself references EF Core, transitively exposes Domain, and publicly exposes `IDomusMindDbContext`, so a project-reference allowlist alone is not an isolation boundary. The MCP project may directly reference Application and shared Contracts only; add deterministic architecture tests for both direct project/package references and compiled assembly/type references, failing if the adapter uses Infrastructure, Domain, EF Core, `IDomusMindDbContext`, or persistence namespaces. The transitive Application dependency graph includes EF Core and Domain; disclose that fact, while ensuring the compiled MCP adapter has no reference to or usage of those persistence/domain types.
- The SDK-owned `/mcp` route is a protocol endpoint, not a conversion of business REST endpoints to Minimal APIs. Existing REST endpoints and controller conventions remain unchanged.
- The adapter has no registered household tools in this slice. No OAuth server, tokens, production user data, AI/LLM library, agent framework, or application write behavior is included.

## Proposed design

Use the official `ModelContextProtocol.AspNetCore` package, pinned initially to stable SDK `2.2.0`, in the `DomusMind.Mcp` library. The official SDK's current transport guidance uses `MapMcp` for ASP.NET Core Streamable HTTP and defaults to stateless HTTP. Configure `StatefulForInitializeClients`: current `2026-07-28` clients stay stateless, while clients using the `2025-11-25` initialization handshake remain interoperable. The legacy handshake path uses session state and therefore requires session affinity in multi-instance deployments; confirm that deployment constraint before enabling older protocol clients beyond this foundation. Re-check package compatibility and security advisories at implementation time. This is an adapter library hosted in-process, not a separate service.

Expose narrowly scoped service-registration and endpoint-mapping extensions so `DomusMind.Api` explicitly opts in and mounts `/mcp`. The registration extension returns the SDK builder so future capability slices can register explicit tool types without a second registry. Do not create a generic tool framework, persistence abstraction, or placeholder application dispatcher. Production registers no tools in this issue. A test-only probe tool demonstrates bounded SDK input/output, cancellation propagation, and dispatch through the existing `IQueryDispatcher` seam; it is not included in the production assembly or tool catalog. Future production tools will use existing application queries/commands and their domain authorization.

The API's existing SPA fallback currently excludes `/api` and `/swagger`; it must also exclude `/mcp` so an unsupported MCP HTTP method cannot receive the SPA document instead of a protocol/HTTP response. Factor the actual endpoint composition into a testable API extension if needed, and exercise it with the SPA present rather than duplicating the route configuration in a test-only host.

Verify the SDK's Streamable HTTP `Origin` handling against the current protocol. If the SDK does not enforce the required behavior, add a narrow middleware/endpoint guard: validate a present `Origin` against an explicit configured allowlist and return 403 for an invalid origin. Do not infer trust from forwarded-host headers. The endpoint remains data-free and tool-free until #229 authorization and #228 product/capability gates are satisfied.

This creates no ProductShape or OpenSpec domain behavior. Existing domain specifications remain authoritative. Record the impact as adapter/architecture-only; do not edit accepted model artifacts or duplicate domain requirements.

## Alternatives considered

- **Put SDK setup directly in `DomusMind.Api`:** smallest file count, but the API already references Infrastructure, so it cannot structurally enforce the issue's adapter dependency boundary. Rejected in favor of a library project.
- **Separate deployable MCP service:** rejected because the owner accepted in-process hosting and existing self-hosted deployment provides one API container; no evidence justifies a second service.
- **Add sample household tools now:** rejected because #229 authorization and #228 capability/product gates have not passed. Tests may register test-only fixtures, but production exposes no tool.
- **Add a generic mediator/tool framework:** rejected; existing application command/query contracts are sufficient for future capability slices.

## Ordered tasks

1. [x] Add `src/backend/DomusMind.Mcp` as a class library with an explicit project-reference allowlist (Application and Contracts only) and official SDK ASP.NET Core package `2.2.0`.
2. [x] Add service-registration and `/mcp` endpoint-mapping extensions; configure SDK Streamable HTTP for stateless current clients plus SDK-managed initialized sessions for down-level clients, and leave production capabilities empty.
3. [x] Integrate the module into `DomusMind.Api` without changing existing REST controller routes or exposing any household capability; update solution and container build inputs.
4. [x] Add an official SDK client/server contract harness. The test-only probe rejects blank and over-32-character input, dispatches through `IQueryDispatcher`, proves cancellation reaches and cancels the in-flight query, and does not leak internal exception details; register it only in the test host. Verify negotiation with both current `2026-07-28` and down-level `2025-11-25` clients. **Plan deviation:** tests live in `DomusMind.Api.Tests` instead of a separate `DomusMind.Mcp.Tests` project so the same harness can exercise actual API endpoint composition, existing REST controllers, and the SPA fallback. Focused architecture checks and SDK-client protocol tests remain isolated under the `Mcp` test namespace.
5. [x] Add protocol transport tests for Streamable HTTP discovery, the empty production tools catalog, structured errors, unsupported methods, and `Origin` validation (configured origin accepted; present invalid origin receives 403; absent Origin remains usable for non-browser clients).
6. [x] Prove the project dependency boundary with an explicit direct-reference allowlist and deterministic compiled assembly/type checks; the adapter does not reference Infrastructure, Domain, EF Core, `IDomusMindDbContext`, or persistence namespaces. The existing Application project has a transitive EF Core/Domain graph, which is disclosed above; the adapter has no production Application dispatch implementation or placeholder framework.
7. [x] Exercise the actual API endpoint-mapping path with static SPA files enabled. An unsupported `/mcp` method does not return the SPA document, invalid Origin returns 403, discovery returns no production tools, error results do not leak internals, and `/api/system/ping` remains reachable.
8. [x] Update the architecture views to reflect the non-deployable adapter and in-process endpoint. Add ADR-0011 for the transport boundary; ADR-0005 remains unchanged and governs business REST/controller endpoints. ProductShape/OpenSpec impact is none; existing domain specifications remain unchanged.

## Test and verification plan

- `dotnet build DomusMind.slnx --configuration Release`.
- `dotnet test DomusMind.slnx --configuration Release`, including new MCP transport contract and dependency-boundary tests.
- Verify direct project/package references and compiled MCP assembly/type references; disclose EF Core and Domain as transitive dependencies of the existing Application project while proving no MCP adapter code/assembly uses them or Infrastructure persistence types.
- Verify protocol behavior with the official SDK for current `2026-07-28` and down-level `2025-11-25` client negotiation.
- Verify no production tool/resource/prompt is registered and no household data is returned without a future authorization slice.
- Check Origin behavior for allowed and invalid values and confirm 403 for an invalid present Origin.
- Exercise the actual API route-composition path with the SPA fallback enabled; verify unsupported `/mcp` methods do not return `index.html` and existing REST controller routes remain reachable.
- Run applicable repository ProductShape/OpenSpec/architecture citation checks if documentation/spec artifacts are changed; do not claim these are application tests.
- Inspect live PR checks and branch rules; zero checks are reported as absent, not passing.

## Implementation evidence (issue 227)

- The production registration exposes an explicit empty `tools/list` result rather than omitting the method; the SDK otherwise returned “method unavailable” when no tools or list handler were registered.
- Origin allowlist values are validated at host startup. Requests without Origin are allowed for non-browser clients; malformed, multiple, path-bearing (including dot-segment paths), or non-allowlisted Origin values are rejected with 403. The raw Origin must match its parsed authority and the allowlist exactly, preventing URI normalization from making an invalid serialized Origin appear trusted. This middleware does not replace trusted-proxy/Host checks.
- The SDK hybrid session mode serves the latest protocol statelessly and supports initialization-handshake clients through session state; the latter require session affinity when the API is scaled to multiple instances.
- The API test host invokes the same `MapDomusMindEndpoints` route composition as production with a temporary SPA index file and a real controller discovery part.
- Verification: Release solution build and all 858 backend tests passed (API 24, Domain 250, Application 525, Infrastructure 59, including all three PostgreSQL tests); API publish and Docker image build passed. The official SDK client explicitly negotiates and asserts current `2026-07-28` and down-level `2025-11-25`. The probe rejects whitespace-only and over-32-character values before dispatch, and proves cancellation reaches an in-flight query.
- Documentation/specification checks: OpenSpec validation passed 10/10; ProductShape validation reported 0 errors and 59 existing low-confidence draft warnings; the managed integration check passed; 816 OpenSpec citations and 81 architecture citations are current with no stale, tampered or unresolved citations. Arc42 frontmatter and Markdown lint passed. GitHub product, docs, backend, web-app and public-site workflow runs are linked in the FF run record.
- Security disposition remains open for existing vulnerable dependencies: NU1903 `Microsoft.Kiota.Abstractions` 1.17.1 (high, transitive API/Infrastructure graph), NU1902 `MessagePack` 2.5.302 (moderate, AppHost), plus repository Dependabot alerts #98 (high `http-cache-semantics`, public site) and #102 (low `KaTeX`, snapshot tooling). The new MCP project has no vulnerable package reported. The Docker legacy-builder and existing web large-chunk notices remain.
- No production tools, resources, prompts, authorization server, household data access, or ProductShape/OpenSpec behavior was added.

## Definition of Done impact

- Issue acceptance criteria are demonstrated by code, tests, and project dependency evidence.
- Implementation is complete with no placeholder capability behavior or silent scope loss.
- Backend build and all relevant backend tests pass.
- Tests confirm that this slice exposes no data-capable production tool, rejects malformed/non-allowlisted Origin headers, and enforces the adapter dependency conformance boundary. This is not a security sandbox or authorization layer; owner security disposition of baseline dependency advisories remains a separate Done gate.
- Architecture conforms to the accepted in-process host decision while preserving controller-based REST APIs and the adapter project boundary.
- No domain/product behavior is introduced; ProductShape/OpenSpec impact is recorded and canonical specifications remain authoritative.
- Documentation and limitations are explicit, including OAuth server and household tools remaining out of scope.
- Independent reviewer approves the final head; owner-only Done judgements remain governed by the lifecycle.

## Dependencies and follow-on gates

- Parent Epic #224 and Product Change `CHG-MCP-HOUSEHOLD-INTEGRATION-001`.
- #226 is integrated into the Epic branch and its in-process host recommendation is accepted.
- #228 proves the initial read-only capability set with test identity; #229 establishes OAuth/OIDC resource protection before any production household data is exposed. This foundation adds neither capability nor OAuth server.
- #230 must not expose `GetMemberAgenda` until the member-family ownership gap recorded in #226 is fixed and covered by a negative isolation test.

## References

- [Official .NET SDK repository](https://github.com/modelcontextprotocol/csharp-sdk)
- [Official SDK v2.2.0 release](https://github.com/modelcontextprotocol/csharp-sdk/releases/tag/v2.2.0)
- [Official SDK Streamable HTTP transport guidance](https://github.com/modelcontextprotocol/csharp-sdk/blob/main/docs/concepts/transports/transports.md)
- [NuGet package metadata: ASP.NET Core SDK 2.2.0](https://www.nuget.org/packages/ModelContextProtocol.AspNetCore/2.2.0)
- [MCP `2026-07-28` specification](https://modelcontextprotocol.io/specification/2026-07-28)
- [MCP Streamable HTTP transport and Origin security](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports#streamable-http)
- [Issue #227](https://github.com/juangcarmona/domusmind/issues/227), [Epic #224](https://github.com/juangcarmona/domusmind/issues/224), and [accepted assessment #226 / PR #267](https://github.com/juangcarmona/domusmind/pull/267)
