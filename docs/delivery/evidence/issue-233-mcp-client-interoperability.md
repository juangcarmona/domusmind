# MCP client interoperability verification evidence (#233)

- Tested commit: `02343f8`
- Branch: `feat/233-mcp-client-interoperability`
- Date: 2026-10-21
- Scope: repository and standards-level verification only; no external account or private deployment credentials were available.

## Reproducible local commands

| Command | Result |
| --- | --- |
| `python3 -m json.tool docs/delivery/ff/233/3f0835bf-0bdd-4fc9-99ee-58be372eb0b3/run.json` | Passed |
| `npx -y markdownlint-cli2@0.23.3 docs/delivery/plans/issue-233-mcp-client-interoperability.md docs/delivery/assessments/issue-233-mcp-client-interoperability.md` | Passed, 0 issues |
| `git diff --check` | Passed |
| `npm exec --yes --package=@fission-ai/openspec@1 --package=@prodshape/cli@0.22.0 -- openspec validate --all` | Passed, 10/10 specs; existing warnings only |
| `npx -y @prodshape/cli@0.22.0 validate` | Passed, 0 errors; 59 existing provenance warnings |
| `prodshape integration update --check` with OpenSpec 1.14.3 on a temporary tool path | Passed; managed files match |
| `prodshape citations verify --provider openspec` | Passed, 816 current; 0 stale, tampered or unresolved |
| `npx -y @prodshape/cli@0.22.0 citations verify docs/architecture --format json` | Passed, 81 current; 0 stale, tampered or unresolved |
| `dotnet restore DomusMind.slnx` | Passed; inherited Kiota/MessagePack advisories reported |
| `dotnet build DomusMind.slnx -c Release --no-restore` | Passed, 0 errors; 6 inherited dependency warnings |
| `dotnet test DomusMind.slnx -c Release --no-build` | Passed: Domain 250, Infrastructure 60, Application 527, API 61; 7 known PostgreSQL tests skipped |

## Existing protocol evidence reused

- #229 covers the DomusMind HTTP OAuth discovery, authorization-code + PKCE, refresh/revocation and current household authorization path.
- #230 covers an authorized read-only request through the official MCP SDK and real application handlers.
- #231 covers server-owned confirmation, idempotency and write authorization.
- #232 covers additional official-client HTTP read-only tool calls and stable output contracts.

These are server/protocol evidence. They do not prove that a named hosted ChatGPT or Claude product account completed its connector flow, nor that a client UI displayed and enforced its write-approval interaction.

## External evidence status

| Path | Status | Required next evidence |
| --- | --- | --- |
| ChatGPT custom MCP | Blocked, not verified | Eligible Business or Enterprise/Edu workspace; connection creation, tool scan, OAuth, refresh, read, and write approval |
| OpenAI Responses API MCP | Blocked, not verified | Integrating application supplies OAuth token; real `server_url` or approved `tunnel_id` request and approval policy evidence |
| Claude remote connector | Blocked, not verified | Real Claude/Claude Desktop connector, OAuth/DCR or configured registration, refresh, read and write approval |
| Claude Code remote HTTP | Blocked, not verified | Real remote connection, exact fixed-port loopback callback, PKCE, interactive/headless behavior |
| Local STDIO | Unsupported for this server slice | Separate local adapter and credential/trust decision |
| Self-hosted/private remote agent | Unsupported for release from this slice; unverified | Deliberate HTTPS/private-overlay topology, TLS/proxy/tunnel reachability, OAuth and household authorization evidence |

## Open release gate

Client-side write approval remains pending because no eligible hosted client account or deployment was available. The server-side confirmation gate is proven by #231 and remains mandatory, but it must not be relabeled as client UI approval. #233 therefore remains in review/in-progress until the owner either supplies the external test prerequisites or explicitly accepts a follow-up release gate.
