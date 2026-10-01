# DomusMind documentation

Each kind of truth has one home.

| Home | Holds | Changes through |
| --- | --- | --- |
| [`product/`](product/README.md) | The product model: actors, journeys, use cases, rules, terms, bounded contexts and requirements (ProductShape) | A Product Change under `product/changes/`, validated with `prodshape validate` |
| [`design/`](design/README.md) | UI guidance for each surface and the public site, subordinate to the product model | Ordinary edits |
| [`architecture/`](architecture/README.md) | How the system realises the product: arc42, twelve sections | Ordinary edits; product-dependent statements cite the model |
| [`adr/`](adr/README.md) | Architecture decision records | A new ADR; accepted records are not rewritten |
| [`devops.md`](devops.md) | Versioning, release, CI workflows and required checks | Ordinary edits |
| [`engineering-lifecycle.md`](engineering-lifecycle.md) | The adopted engineering lifecycle (Definition of Ready and Done) | The lifecycle's own amendment process |

Behaviour specs live in [`openspec/specs/`](../openspec/specs), bound to the product model by citations. The Product Snapshot, a browsable page of the whole model, is attached to every run of the `product-snapshot` workflow.

## History

Until October 2026 the documentation was hand-written under numbered folders (`docs/00_product` to `docs/09_roadmap`). It was recovered into the product model, arc42 and ADRs, then retired; commit `623ce92` is the last one that contains all of it. The model's `provenance` fields and some OpenSpec notes still name those original paths.
