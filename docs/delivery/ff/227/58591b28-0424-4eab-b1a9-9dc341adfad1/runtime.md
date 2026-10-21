# Runtime evidence: issue #227

- Captured: 2026-10-20 05:23 UTC
- Image: `domusmind:issue-227` (`sha256:0909450cae7cc81f4177e87375b919ae7134b35c2e5f4dcd3bb271917f40bf5b`), built from the worktree's runtime source for commit `705c9ab3befe961393a4a6941a642c7bcd6c70bb`; later changes before audit were test/evidence only.
- Runtime: task-scoped container on `127.0.0.1:18087`; disposable PostgreSQL test database
- Authentication: synthetic test configuration only; no user or production credentials

| Request | Result | Observation |
| --- | --- | --- |
| `POST /mcp`, current MCP protocol `2026-07-28`, `tools/list`, allowed `Origin: https://trusted.example` | `200 OK`, `text/event-stream` | Valid MCP response with an intentionally empty `tools` array and server info `DomusMind.Api`. |
| Same MCP request with malformed dot-segment `Origin: https://trusted.example/a/..` | `403 Forbidden` | URI normalization cannot turn a path-bearing Origin into an allowed authority. |
| `GET /mcp` with `MCP-Protocol-Version: 2026-07-28` | `405 Method Not Allowed` | Protocol response permits POST only; did not fall through to the SPA. |
| `GET /api/system/ping` | `200 OK` | Existing REST endpoint remains reachable; response `{"status":"ok"}`. |

The requests used the request metadata and required HTTP headers defined by the current [Streamable HTTP specification](https://modelcontextprotocol.io/specification/2026-07-28/basic/transports/streamable-http). The disposable runtime and PostgreSQL containers were stopped after capture. This verifies transport, routing, and Origin behavior only; it does not assert authentication or household authorization, which are explicitly outside issue #227.

Sanitized transcript excerpts:

```text
POST /mcp
MCP-Protocol-Version: 2026-07-28
Mcp-Method: tools/list
Origin: https://trusted.example
HTTP/1.1 200 OK
event: message
data: {"result":{"tools":[],"ttlMs":0,"cacheScope":"private","resultType":"complete",...},"id":1,"jsonrpc":"2.0"}

POST /mcp (Origin: https://trusted.example/a/..)
HTTP/1.1 403 Forbidden

GET /mcp (MCP-Protocol-Version: 2026-07-28)
HTTP/1.1 405 Method Not Allowed
Allow: POST

GET /api/system/ping
HTTP/1.1 200 OK
{"status":"ok"}
```
