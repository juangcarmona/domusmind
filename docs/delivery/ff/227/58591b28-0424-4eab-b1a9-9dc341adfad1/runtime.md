# Runtime evidence: issue #227

- Captured: 2026-10-20 05:23 UTC
- Image: `domusmind:issue-227`
- Runtime: task-scoped container on `127.0.0.1:18087`; disposable PostgreSQL test database
- Authentication: synthetic test configuration only; no user or production credentials

| Request | Result | Observation |
| --- | --- | --- |
| `POST /mcp`, current MCP protocol `2026-07-28`, `tools/list`, allowed `Origin: https://trusted.example` | `200 OK`, `text/event-stream` | Valid MCP response with an intentionally empty `tools` array and server info `DomusMind.Api`. |
| Same MCP request with malformed dot-segment `Origin: https://trusted.example/a/..` | `403 Forbidden` | URI normalization cannot turn a path-bearing Origin into an allowed authority. |
| `GET /mcp` with `MCP-Protocol-Version: 2026-07-28` | `405 Method Not Allowed` | Protocol response permits POST only; did not fall through to the SPA. |
| `GET /api/system/ping` | `200 OK` | Existing REST endpoint remains reachable; response `{"status":"ok"}`. |

The requests used the request metadata and required HTTP headers defined by the current [Streamable HTTP specification](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports/streamable-http). The disposable runtime and PostgreSQL containers were stopped after capture. This verifies transport, routing, and Origin behavior only; it does not assert authentication or household authorization, which are explicitly outside issue #227.
