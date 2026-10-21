# MCP client interoperability evidence (#233)

Status: draft, pending external-client credentials and a deliberately reachable test deployment.

## Environment boundary

This repository environment has no ChatGPT workspace administration, Claude account, production OAuth client credentials, public DNS/TLS endpoint, or private MCP tunnel authorization. No external account or private service is exposed to manufacture a green demonstration. The absence is an explicit release blocker, not a claim that the clients are incompatible.

## Available evidence

The following repository evidence is reusable for this child:

- #226 records the client-specific transport, registration, reachability, and plan limitations.
- #229 proves protected-resource discovery, OAuth Authorization Code + PKCE, token/resource validation, refresh/revocation behavior, and current household authorization at the DomusMind HTTP boundary.
- #230 proves a real authorized read-only MCP flow through the official .NET SDK and application handlers.
- #231 proves server-owned confirmation, approval, idempotency and write authorization; a model/client approval prompt is not treated as authority.
- #232 adds four additional read-only tools with official-client HTTP coverage and stable contracts.

These prove the server-side protocol and authorization contract. They do not prove product-account availability, client plan eligibility, or deployment reachability from a hosted AI service.

## Client dispositions

| Profile | Evidence status | Blocker or next test |
| --- | --- | --- |
| ChatGPT custom MCP | Protocol-supported / client unverified | Run in an eligible Business or Enterprise/Edu workspace: create connection, scan tools, complete OAuth, refresh after expiry, invoke a read tool, and verify write approval. |
| OpenAI Responses API MCP | Protocol-supported / client unverified | Run a Responses API request with `server_url` or an approved `tunnel_id`; provide the OAuth access token through the integrating application and verify approval policy separately. |
| Claude remote connector | Protocol-supported / client unverified | Add the remote URL in Claude/Claude Desktop, complete OAuth/DCR, verify refresh, read, write confirmation and plan/workspace restrictions. |
| Claude Code remote HTTP | Protocol-supported / client unverified | Exercise remote OAuth from an interactive client and separately record headless limitations. |
| Local STDIO | Deferred / not supported by current server slice | Requires a separate local adapter and credential/trust decision; no broad PAT is inferred from the current OAuth resource. |
| Custom/self-hosted agent | Conditional | Use the deployment runbook with a real HTTPS or private overlay path; retain the same OAuth and household authorization tests. |

## Safety constraints

- No credentials, access tokens, refresh tokens, client secrets, private endpoint URLs, household data, or model prompts are committed.
- Hosted-client tests must use synthetic household data and disposable OAuth grants.
- Any write test must confirm both the client-side approval interaction and the server-side confirmation record; disabling a client prompt must not bypass DomusMind authorization.
- Hosted-client test results must include the exact date, product surface, plan/workspace, client version, deployment topology, and evidence links because these behaviors change independently of MCP protocol revisions.

## Release decision

Until the external tests are run, DomusMind can claim MCP protocol compatibility and a tested server-side OAuth/read/write contract. It must not claim verified ChatGPT or Claude product compatibility, broad self-hosted remote support, or local STDIO support.
