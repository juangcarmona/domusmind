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

### Start with an area

| Household question | Entry point |
| --- | --- |
| Who belongs to the household and who can use the app? | [Household and people](model/domain/bounded-contexts/bc-family.md) |
| Who is responsible for an Area? | [Close ownership gaps](model/journeys/jrn-areas-close-ownership-gaps.md) |
| What is happening and what needs attention today? | [Review today](model/journeys/jrn-product-what-matters-today.md) |
| How does work move from pending to done? | [Tasks](model/domain/lifecycles/lc-task.md) and [Routines](model/domain/lifecycles/lc-routine.md) |
| How do we reuse a checklist? | [Lists](model/journeys/jrn-lists-reuse-a-list.md) |
| How do meals become a shopping list? | [Plan meals and shopping](model/journeys/jrn-meals-plan-the-week.md) |
| How does a Chat request become a reviewed action? | [Chats, Inbox and Calendar](model/journeys/jrn-chat-review-household-action.md) |
| What happens to a captured suggestion? | [Inbox proposals](model/domain/lifecycles/lc-inbox-proposal.md) |
| What household things and obligations are in scope? | [Household Assets](model/journeys/jrn-assets-keep-household-ready.md) |

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

## Write for people

Use Humanizer,
[Taste](https://github.com/Leonxlnx/taste-skill/blob/main/skills/taste-skill/SKILL.md)
and [Plain English](https://github.com/b1rdmania/claude-plain-english-skill/blob/main/SKILL.md)
as editorial review inputs. Name the person who acts, state the outcome directly
and put the information readers need first. Keep obligations, permissions,
conditions and unresolved decisions explicit. Product and design decisions retain
their existing authority.

Use a requirement title to name the required outcome and a use-case title to name
the person's action. Keep them separate when the requirement states an obligation
and the use case explains its context and flow. A structured behaviour gives one
context, one stimulus and outcomes that hold together; split alternative actions
or outcomes into separate examples.

The [model review](reviews/2026-11-13-model-clarity.md) records the lifecycle rollout,
the review of all 181 original examples and the remaining owner questions.
