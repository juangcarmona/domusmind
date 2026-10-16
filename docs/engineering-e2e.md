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

The browser scenario completes onboarding at desktop width, navigates the
desktop shell, then checks the Agenda and navigation drawer at 390px phone
width. It also checks that the document does not overflow horizontally. This
is a deliberately small baseline; the coverage does not claim full behavior
coverage of every surface or phone layout.

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
