# FF run 9894243e-10bd-4ec4-a255-4d986214b8e9

- Work item: #232
- Branch: `feat/232-expand-mcp-capabilities`
- Starting head: `ae2d3754ffbd5ef3c1bc4cd554edfe2b432eecc5`
- Epic: [#224](https://github.com/juangcarmona/domusmind/issues/224)
- Draft PR: [#273](https://github.com/juangcarmona/domusmind/pull/273)
- Status: proposal awaiting independent review

Fast Forward started for the next dependency-ready MCP capability slice after confirmed household writes (#231). The scope must select a coherent, evidence-backed subset of list, meal, routine, recipe, responsibility-area, task and calendar capabilities; it must not mechanically expose every endpoint.

The proposal selects household-shared list reads, one-week meal-plan reads, routine reads, and responsibility-area reads. Private-list reads, task queries, recipe operations, new writes, Resources, Prompts, and remote interoperability remain explicitly deferred. The proposal correction binds list detail to the MCP household, validates meal weeks against household settings, and specifies household-first meal ordering at the MCP boundary.

Independent proposal review at `477154b` requested changes; those changes are present in `3c4dc6a`. The corrected proposal was independently approved at `7323961`; the remaining P2 wording/order requests are present in `48bc109`. The proposal gate is complete; implementation may begin.
