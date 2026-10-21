# Proposal: validate MCP client interoperability and deployment paths

- Work item: [#233](https://github.com/juangcarmona/domusmind/issues/233)
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Product Change: [`CHG-MCP-HOUSEHOLD-INTEGRATION-001`](../../product/changes/active/chg-mcp-household-integration/change.md)
- Definition PR: [#225](https://github.com/juangcarmona/domusmind/pull/225)
- Depends on: #226, #227, #229, #230, #231, #232

## Proposal

Turn the existing MCP protocol and OAuth implementation into an honest client/deployment support contract. The result is a versioned support matrix, a repeatable interoperability runbook, and evidence for the paths that can be tested in the available environment. The work does not claim universal client compatibility and does not expose credentials or a private endpoint for a demonstration.

The server remains the existing in-process DomusMind API MCP resource. Client-specific behavior is recorded at the integration boundary; household authorization, confirmation, and domain validation remain server-side and authoritative.

## Scope

1. Document and verify the MCP resource URL, protected-resource metadata, authorization-server discovery, OAuth Authorization Code + PKCE flow, refresh behavior, consent, read/write scope behavior, and server-side confirmation semantics.
2. Maintain a client matrix for ChatGPT custom MCP connections, OpenAI Responses API MCP, Claude remote connectors, Claude Code remote HTTP, local STDIO clients, custom remote agents, and self-hosted agents.
3. Exercise the available standards-level path with a real HTTP client against the deployed/test-host MCP endpoint and existing OAuth/read-only tools.
4. Define the exact prerequisites for public HTTPS, reverse proxies, TLS, forwarded-host handling, private overlays, and OpenAI Secure MCP Tunnel. Treat a tunnel as a client-specific reachability option, not a replacement for authorization-server reachability.
5. Record unavailable external-account or deployment evidence as a precise blocker and identify the smallest follow-up validation required before claiming support.

## Non-goals

- No new MCP tools, prompts, resources, transport, OAuth provider, or authorization model.
- No local STDIO adapter or Personal Access Token implementation.
- No public endpoint, tunnel, provider account, production credential, or certificate is created for this issue.
- No claim that ChatGPT, Claude, or any other client is supported solely because it implements MCP.
- No change to server-side authorization, write confirmation, household selection, or domain handlers.

## Support matrix

The status vocabulary is deliberate:

- **Verified**: a reproducible test was run against a real endpoint and its evidence is linked.
- **Protocol-supported / client unverified**: the server contract and local standards-level path are covered, but the named product account/deployment was not available.
- **Conditional**: support depends on a product plan, workspace policy, registration mode, network path, or operator configuration.
- **Not supported**: the current DomusMind delivery does not provide the required transport or credential boundary.

The current DomusMind server supports a pre-registered public OAuth client whose exact redirect URIs come from `Mcp:OAuth:RedirectUris`; it does not implement Dynamic Client Registration (DCR) or Client-Id Metadata Documents (CIMD). DCR/CIMD are client/provider interoperability options, not capabilities to imply from the MCP protocol. A client requiring one of them is **Conditional** or blocked until a separately approved registration capability exists.

| Client/profile | Transport and reachability | Auth/registration expectations | Current disposition | Required evidence before release claim |
| --- | --- | --- | --- | --- |
| ChatGPT custom MCP connection | Remote HTTPS MCP URL; private Secure MCP Tunnel may be used where the target workspace/product supports it | OAuth discovery, PKCE, resource-bound access tokens; refresh tokens are needed for durable connectivity | Conditional; client-account test is unverified in this environment | Real connection creation/tool scan, OAuth consent, refresh, read call, and write approval in an eligible Business or Enterprise/Edu workspace |
| OpenAI Responses API MCP | `server_url` for remote HTTPS or `tunnel_id` for supported private paths | The integrating application supplies the OAuth bearer token; this is separate from ChatGPT connector registration | Protocol-supported / client unverified | Real Responses API request with the selected MCP tool, explicit approval policy, token audience, and tunnel/public reachability evidence |
| Claude remote connector | Streamable HTTP over HTTPS; remote public reachability or a supported private route | OAuth/DCR or a configured client path, refresh support, provider discovery and callback compatibility | Conditional; client-account test is unverified | Add connector, complete OAuth, refresh, invoke read-only tools, and document plan/workspace behavior |
| Claude Code remote HTTP | Remote HTTP reachable from the developer machine | MCP OAuth discovery/DCR or pre-registered configuration, depending on client capability | Protocol-supported / client unverified | Run a remote `claude` connection through OAuth and record interactive/headless behavior |
| Local STDIO client | Child process on the operator machine; no remote HTTPS implied | Environment/process credentials; HTTP OAuth is not the default STDIO boundary | Not supported by this server slice; deferred separate adapter | Separate product decision and local adapter with process/trust/secret tests |
| Custom remote agent | Streamable HTTP over HTTPS, private overlay, or deliberate tunnel | Client-specific OAuth registration; interactive Authorization Code + PKCE preferred | Conditional | Repeatable operator runbook, TLS/proxy checks, discovery/token/read/write tests, and security review |
| Self-hosted agent on private infrastructure | Private network or outbound tunnel; a cloud client cannot reach LAN-only MCP automatically | Client-specific OAuth or a separately approved local identity | Conditional; no automatic port exposure | Document topology, firewall, DNS, TLS, proxy trust, tunnel, backup, and revocation evidence |

## Verification protocol

Every client test records:

- client/product surface, version or release channel, plan/workspace and configuration;
- MCP endpoint and transport, without recording tokens or private URLs in committed evidence;
- protected-resource metadata and authorization-server discovery results;
- registration mode, redirect URI, PKCE method, requested resource and scopes;
- consent, access-token expiry, refresh and revocation behavior;
- tool discovery, read-only tool invocation, write approval/confirmation and server-side denial;
- network path, TLS/reverse-proxy/forwarded-host behavior, latency and failure mode;
- household/member identity and authorization result using synthetic test data only.

The minimum standards-level test is an OAuth-protected read-only request through the actual MCP HTTP boundary, not a direct handler call. A client-specific claim requires the named client to complete the same flow.

For the OpenAI Responses API, the integrating application performs OAuth registration, authorization, token storage and refresh; the Responses API request receives the resulting bearer token and does not turn a ChatGPT connector test into an API-client test. For clients that require a pre-registered public client, add the exact callback to `Mcp:OAuth:RedirectUris` in the disposable test environment. For Claude Code remote OAuth, distinguish the remote MCP and authorization endpoints, which require the deployment's HTTPS boundary, from the native client's loopback callback: use the documented fixed callback port, register the exact loopback URI, use PKCE, and reject every unregistered redirect.

## Deployment runbook contract

### Public HTTPS

1. Publish the stable MCP resource URL and keep the resource identifier identical across protected-resource metadata, authorization requests, token audience/resource claims, reverse-proxy routing, and client configuration.
2. Terminate TLS with a maintained certificate and forward only the headers explicitly trusted by the deployment.
3. Configure the authorization server's redirect allow-list with exact URIs; remote MCP and authorization endpoints remain HTTPS-only. A native client loopback callback may use the client-documented local HTTP loopback address and fixed port, but it must be explicitly registered, protected by PKCE, and never generalized into a public callback.
4. Verify `Origin`, host, forwarded-host, authorization, timeout, cancellation and rate-limit behavior at the public boundary.

### Private deployment

1. Prefer a private overlay or a supported client-specific tunnel over opening a household host directly to the internet.
2. For OpenAI Secure MCP Tunnel, the tunnel client must reach the private MCP server and make outbound HTTPS to the OpenAI control plane; the tunnel does not automatically make the OAuth authorization server reachable.
3. Keep the MCP server and tunnel client inside the same trust boundary, use separate credentials, and record tunnel health without logging secrets.
4. A private route is not evidence of client authorization. OAuth discovery, consent, token validation and current DomusMind household authorization remain required.

## Ordered delivery and verification tasks

1. Reconcile this matrix with the existing #226 and #229 assessment; record the current OAuth registration boundary and the client-specific status for the assessment date.
2. Verify the standards-level path through the actual `/mcp` HTTP endpoint using the existing OAuth test flow, then exercise one read-only tool and one denied/expired/revoked case. Reuse the existing tests; do not add a parallel client harness unless a gap is demonstrated.
3. Validate public-HTTPS and private-route prerequisites using a disposable reverse-proxy/tunnel configuration or document the unavailable deployment prerequisite precisely.
4. Run ChatGPT and Claude product-account tests when the owner supplies eligible accounts/workspace access; otherwise record each as blocked with the exact next test, not as passed.
5. Update the evidence assessment and operator-facing MCP documentation with the observed client version/plan, topology, registration mode, refresh, approval and failure behavior.
6. Run `dotnet build DomusMind.slnx -c Release --no-restore`, `dotnet test DomusMind.slnx -c Release --no-build`, `npx -y markdownlint-cli2@0.23.3` for changed Markdown, and the repository's docs/product validation commands. Record actual outputs in the FF evidence.

## Artifact and acceptance mapping

| Issue acceptance criterion | Artifact/evidence | Passing condition |
| --- | --- | --- |
| Support matrix separates client surface/version from protocol requirements | This plan, `docs/delivery/assessments/issue-233-mcp-client-interoperability.md` | Every row has transport, auth/registration, disposition, and required evidence; unverified rows are not reported as supported. |
| ChatGPT and Claude real path or precise blocker | Assessment client dispositions and linked external test evidence when available | Each product is **Verified** only after its named client completes the flow; absent accounts/deployment are recorded as explicit blockers. |
| Local STDIO and self-hosted paths are separate | Support matrix and deployment runbook | STDIO is not inferred from HTTP OAuth; private routes document trust, TLS, tunnel and credential boundaries. |
| External endpoint exposure is deliberate | Public/private deployment runbook | No public endpoint or credential is created by this issue; operator prerequisites are explicit. |
| Server-side confirmation remains authoritative | #231 evidence plus client write-approval record | Client approval is supplementary; the DomusMind confirmation/authorization path is independently required. |
| Client limitations appear in support docs | Matrix, assessment, and operator hand-off | Product/plan/version, registration, refresh, reachability and failure limitations are recorded with assessment date. |

## Acceptance evidence

- The support matrix distinguishes normative MCP/OAuth requirements from client-specific behavior, versions/plans, and unsupported claims.
- The repository contains a repeatable standards-level HTTP/OAuth/read-only verification procedure and records its result.
- ChatGPT and Claude paths are each either evidenced by a real client test or marked with the precise unavailable credential/deployment blocker and the exact next test required.
- Local STDIO and self-hosted private paths are separate profiles; no credential is copied into source, committed configuration, logs, or evidence.
- Server-side authorization is tested independently of client prompts; client approval never grants household authority. Named-client write-approval evidence remains pending and is a release gate.
- Public HTTPS, reverse-proxy, TLS, private-route, tunnel and reachability constraints are documented for operators.
- The plan and evidence link back to the canonical Product Change and Epic without changing existing domain ownership.

## Dependencies and hand-off

- #229 supplies OAuth/resource protection and discovery.
- #230 and #232 supply read-only operations to exercise.
- #231 supplies confirmed-write behavior and approval semantics to verify without weakening server-side confirmation.
- #234 owns the final threat review, privacy-aware telemetry, contract-test expansion, and production release gate.

If a client requires a server capability not currently supported, create a separate product/architecture issue instead of broadening this slice silently.

## References

- [MCP transport and authorization assessment](../assessments/issue-226-mcp-architecture-assessment.md)
- [MCP OAuth security review](../assessments/issue-229-mcp-security-review.md)
- [OpenAI: MCP servers and Responses API](https://developers.openai.com/api/docs/guides/tools-connectors-mcp)
- [OpenAI: connecting custom MCP servers in ChatGPT](https://help.openai.com/en/articles/12584461-connecting-custom-mcp-servers-in-chatgpt)
- [OpenAI: Secure MCP Tunnel](https://developers.openai.com/api/docs/guides/secure-mcp-tunnels)
- [Anthropic: building connectors](https://claude.com/docs/connectors/building)
- [Claude Code: MCP and fixed OAuth callback port](https://code.claude.com/docs/en/mcp#use-a-fixed-oauth-callback-port)
- [Anthropic: custom connector security and setup](https://support.anthropic.com/en/articles/11175166-about-custom-integrations-using-remote-mcp)
- [MCP authorization](https://modelcontextprotocol.io/specification/2026-07-28/basic/authorization)
- [MCP transports](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports)
