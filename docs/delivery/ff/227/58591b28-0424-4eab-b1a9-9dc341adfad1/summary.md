# Fast Forward run: issue #227

- Run ID: `58591b28-0424-4eab-b1a9-9dc341adfad1`
- Status: running
- Branch: `feat/227-establish-dotnet-mcp-adapter-foundation`
- Starting commit: `320526d4e3e49e5358d571e09ff50fe7a01e6086`
- Executor: GPT-5 (active Codex model)
- Independent reviewer: `gpt-6.1-sol`
- Correction limit: 3 rounds per gate
- Parent Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Work item: [#227](https://github.com/juangcarmona/domusmind/issues/227)

## Stage evidence

- `refine`: Ready. Issue #227 names affected household members, product owner/maintainers, and external MCP client users; records accepted #226 host direction; defines exclusions and six verifiable criteria.
- `propose`: In progress. Independent review requested two bounded fixes: explicitly handle Application's transitive EF/Domain graph with assembly/type dependency tests, and test actual API route composition so unsupported `/mcp` methods cannot fall through to the SPA. Both corrections are now reflected in the plan. The plan uses a `DomusMind.Mcp` library hosted inside `DomusMind.Api`, with no production tools or household data in this slice. Authorization-server placement remains open for #229.
- Worktree setup: dedicated worktree created from the updated Epic branch `feat/224-mcp-household-assistants` at `320526d`; `dotnet restore DomusMind.slnx` passed with pre-existing NU1902/NU1903 advisories. No local secrets or untracked configuration were needed.

## Scope boundary

No application implementation is included in the proposal commit. The implementation stage must use the official .NET SDK, keep adapter project references away from Infrastructure/EF Core, expose no production tools, and verify Streamable HTTP Origin behavior.
