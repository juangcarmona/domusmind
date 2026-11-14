# Product definition

DomusMind's product definition describes how people coordinate their household.
It uses the PDaC `v1alpha2` contract, checked by the ProductShape version in the
root lockfile.

## Read the model

Start with the household outcome you want to understand. Follow its references
to the rules, states and examples that explain the behavior.

| Read | To understand |
| --- | --- |
| [Journeys](model/journeys/) | The outcome a person works toward across several actions |
| [Use cases](model/use-cases/) | Who acts, what they do and what result they expect |
| [Business rules](model/business-rules/) | The conditions that govern those actions |
| [Domain terms and contexts](model/domain/) | What household concepts mean and where they belong |
| [Domain lifecycles](model/domain/lifecycles/) | The states a concept can enter and its permitted transitions |
| [Structured behaviours](model/behaviours/) | Concrete examples of context, one stimulus and observable results |
| [Requirements](model/requirements/) | Product obligations and their verification examples |

Requirements and use cases have a many-to-many relationship: one requirement can
span several use cases, and one use case can support several requirements.
Their relationships follow product meaning. Titles and identifiers do not set
their cardinality.

An event can be the stimulus or an observable result of a behavior. A lifecycle
records the resulting state transition. A structured behaviour can refer to that
transition with `covers-transition`; it continues to identify its governing use
case, rule or constraint through `illustrates`.

Generate the browsable Product Explorer with `npm run product:build`. Its output
is `.product/generated/explorer.html`.

## Change the definition

- `model/` holds the accepted Product Definition (the baseline).
- `changes/active/` holds live Product Changes, each with its complete proposed future state.
  `changes/completed/`, `changes/rejected/` and `changes/superseded/` hold the change history,
  one directory per terminal status, and are inert; they materialize when the first change is
  applied or archived.
  In this repository an applied change keeps only its `change.md`: its `proposed/` tree is
  identical to what it wrote into `model/`, so it is removed after apply and stays in git history.

The definition changes through exactly one mechanism: a Product Change, validated as an overlay,
approved by a human, applied with `prodshape change apply`, and accepted when a human merges the
pull request carrying the result. Product-definition work and implementation work may share that
pull request or proceed at different times, but apply, acceptance and delivery remain distinct.

Validate with `prodshape validate`. Print an authoring template with
`prodshape template <kind>` and the allowed frontmatter with `prodshape schema <kind>`;
`prodshape init --full` installs the template library under `.product/templates/`.

Artifact status describes product acceptance: `active` is current intent;
`draft` needs a decision; `deprecated` remains addressable while consumers move;
`retired` is historical. Implementation and deployment are tracked separately.

The local lifecycle template follows `prodshape schema domain-lifecycle`. The
0.26.0 template command still lists the earlier artifact kinds, so use
`.product/templates/domain-lifecycle.md` when authoring a lifecycle.
