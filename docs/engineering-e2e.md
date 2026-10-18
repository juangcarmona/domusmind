# Browser end-to-end reports

The `engineering-site` workflow runs the selected browser journey on pull
requests, manual dispatch, and nightly. The nightly gate skips when the current
`main` commit already has a published report. Manual dispatch always reruns it.
The workflow starts an ephemeral PostgreSQL service, runs the API and Vite app,
and uses only generated household names and credentials in that disposable
database.

## Initial journey selection

| Journey | Product source | Browser evidence |
| --- | --- | --- |
| First-run setup, household creation, adding the manager and a member, then reaching today's household Agenda | [Start a household and reach a working Agenda](product/model/journeys/jrn-product-onboard-household.md); [Household Creation](../openspec/specs/family/spec.md#requirement-household-creation); [Agenda Default Entry State](../openspec/specs/web-app/spec.md#requirement-agenda-default-entry-state) | [`household-onboarding.e2e.ts`](../src/web/app/e2e/household-onboarding.e2e.ts) |
| Navigate from Agenda to Lists with the shared shell retained | [App Shell and Navigation](../openspec/specs/web-app/spec.md#requirement-app-shell-and-navigation), scenario “User navigates from Agenda to Lists” | [`household-onboarding.e2e.ts`](../src/web/app/e2e/household-onboarding.e2e.ts) |
| Create and update a household Plan on a phone and verify its Agenda projection | [What matters today?](product/model/journeys/jrn-product-what-matters-today.md); [Agenda](../openspec/specs/web-app/spec.md) | [`household-onboarding.e2e.ts`](../src/web/app/e2e/household-onboarding.e2e.ts) |
| Assign a recipe to a meal slot, request the shared shopping list, and verify its ingredients and Agenda projection | [Plan the week](product/model/journeys/jrn-meals-plan-the-week.md) | [`household-onboarding.e2e.ts`](../src/web/app/e2e/household-onboarding.e2e.ts) |
| Capture Inbox text, review proposals, verify that approval creates a Plan while rejection creates no timeline event, and ask an explicit `@dm` question in household Chat | [Capture and review Inbox items](product/model/use-cases/uc-inbox-capture-and-review.md); [Ask the assistant in Chat](product/model/use-cases/uc-chat-ask-assistant.md); [Inbox](../openspec/specs/inbox/spec.md); [Chats](../openspec/specs/chats/spec.md) | [`household-onboarding.e2e.ts`](../src/web/app/e2e/household-onboarding.e2e.ts) |
| Capture a reusable list item, check and uncheck it, and observe a live item update across two browser sessions | [Reuse a list](product/model/journeys/jrn-lists-reuse-a-list.md) | [`household-onboarding.e2e.ts`](../src/web/app/e2e/household-onboarding.e2e.ts) |

The browser scenario completes onboarding and desktop navigation, exercises
Plan creation and editing at 390px phone width, then covers Inbox review,
household Chat, meal planning and shared list reuse. It checks that ordinary
Chat messages do not invoke the assistant and that an explicit `@dm` question
does. It also checks that the document does not overflow horizontally at phone
width. The suite focuses on these accepted journeys and does not claim
exhaustive behavior coverage of every surface or phone layout.

## Published reports and recovery

- Report index: [`/e2e/`](https://juangcarmona.github.io/domusmind/e2e/)
- Stable latest report: [`/e2e/latest/`](https://juangcarmona.github.io/domusmind/e2e/latest/)
- Current run metadata: [`/e2e/latest.json`](https://juangcarmona.github.io/domusmind/e2e/latest.json)
- Dated reports: `/e2e/runs/<run-id>-<commit-sha>/`

Each report records commit, workflow run, UTC creation time, and every recorded
journey outcome. Passing runs include desktop and phone screenshots, trace
evidence, and the Playwright HTML report; failed runs retain their produced
evidence and remain failed in Actions. A missing or empty test result is
published as `missing-evidence` and cannot make a run green.

Pages retains the newest 14 dated reports. The Actions artifact named
`e2e-report-<run-id>` is the recovery copy and has a 14-day artifact retention.
The workflow has read-only repository access for browser execution; only the
separate Pages deploy job receives deployment permissions.
