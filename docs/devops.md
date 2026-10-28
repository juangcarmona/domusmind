# DomusMind DevOps

## Lighthouse workflow pilot

The repository pins `@juangcarmona/lighthouse` in the root `package-lock.json` and declares the `dotnet-backend`, `react-web-platform`, `web-bff`, and `visual-theme` profiles in `lighthouse.json`. Run `npm ci` at the repository root and `npm run verify:lighthouse` to check the installed Foundation. `LIGHTHOUSE.md` describes the generated artifacts. The canonical DomusMind lifecycle and its `status:*` labels remain in [engineering-lifecycle.md](engineering-lifecycle.md); Lighthouse capability labels are additional routing signals.

The installed SDLC projections cover `triage`, `refine`, `propose`, `implement`, and `apply-review`. Bastion has a dedicated repository runner, `bastion-domusmind-gh-aw`, labelled `lighthouse-workflow`, with a private operator-owned `LIGHTHOUSE_ENGINE_CONFIG` and the shared engine lock. The operator profile is the one used by Lighthouse and SoFa: the agent runs in a bounded, non-privileged Docker container without the host Docker socket, with an OpenCode authentication volume and outbound network. It does not provide AWF network filtering or credential proxy isolation. `LIGHTHOUSE_AUTOMATION_ENABLED` remains unset until the workflows land on the default branch, the runner profile is verified, and the owner and forge gates pass. `integrate` and `review` are not enabled by this adoption; existing human and branch gates still apply.

After integration, enable the repository variable and dispatch one owner-supervised `refine` route on an eligible issue. Confirm that the router, dedicated runner, validated stage result and publisher all complete before closing the adoption issue. If the route fails, delete the variable and keep the issue open while correcting the failure. The activation and rollback commands are `gh variable set LIGHTHOUSE_AUTOMATION_ENABLED --body true`, `gh workflow run "Lighthouse SDLC router" -f issue=<issue-number> -f stage=refine`, and `gh variable delete LIGHTHOUSE_AUTOMATION_ENABLED`.

## Development OIDC BFF pilot

`DomusMind.AppHost` starts a local Keycloak container, imports `infra/keycloak-dev/domusmind-dev-realm.json`, and enables the BFF only for the API's Development environment. Open the Vite app at `http://localhost:61546`. The test identities are `developer` and `development-member`, each with the development-only password `DevelopmentOnly123!` and verified emails `admin@domusmind.local` and `member@domusmind.local`. On a fresh database, initialize the application administrator with the first email through the first-run screen before signing in; Aspire creates the second local test account with an unusable random password. Neither identity has production authority.

The browser receives a Secure, HttpOnly, SameSite=Lax cookie that references an in-memory server-side ticket. The ticket store supports one development API process and revokes tickets on logout; this is not a production session-store decision. The API never returns OIDC tokens to the web app; unsafe API requests require a same-origin `Origin` and an antiforgery token. Local token endpoints return 404 in BFF mode. Keycloak asks the user to confirm its own logout because the BFF does not retain an ID token. Vite forwards its public host and scheme from loopback; any remote preview needs an HTTPS reverse proxy and an explicitly registered redirect URI. The current production runtime still uses local JWT; the approved OIDC product contract is tracked for implementation in [issue #281](https://github.com/juangcarmona/domusmind/issues/281). See [ADR 0011](adr/0011-pilot-oidc-bff-in-development.md) for the development pilot.

This document explains how DomusMind is versioned, built, validated, released and operated, which together form the application lifecycle. It does not cover where the software runs or how its parts map onto infrastructure; see the [Deployment View](architecture/07-deployment-view.md) for that. The installation guide shipped with every release is [`deploy/README.md`](../deploy/README.md).

## Versioning

DomusMind uses semantic versioning with explicit prerelease lanes. [`release.yml`](../.github/workflows/release.yml) accepts only these shapes:

- `MAJOR.MINOR.PATCH-beta.N` (testing release)
- `MAJOR.MINOR.PATCH-rc.N` (release candidate)
- `MAJOR.MINOR.PATCH` (stable, the intended upgrade target)

| Segment | When it changes |
| --- | --- |
| MAJOR | Breaking upgrade or a required manual action, including breaking schema changes |
| MINOR | Significant capability increment |
| PATCH | Bug fix, UX refinement, packaging or operational hardening |

[`prepare-release.yml`](../.github/workflows/prepare-release.yml) is a manual helper. Given a lane (`beta`, `rc` or `stable`) and a bump (`patch`, `minor` or `major`), it computes the next version from the latest stable tag and the existing lane tags, then writes the version and the `prerelease` flag to the run summary. It does not release anything.

## Image Channels

| Channel | Producer | Tags | Mutability |
| --- | --- | --- | --- |
| Edge | [`docker-edge.yml`](../.github/workflows/docker-edge.yml), on every push to `main` that touches backend, web app, compose or release workflow files | `edge` | Moves on every build |
| Release | [`release.yml`](../.github/workflows/release.yml), manual | `v<version>`, `<version>`, `sha-<shortsha>` | Immutable |

Both channels publish to `ghcr.io/<owner>/domusmind` and build from the same Dockerfile, sharing one GitHub Actions build cache. Use `edge` only to test the current tip of `main`. Installations pin an exact release version. The release workflow never publishes `latest`, `edge` or `MAJOR.MINOR` tags.

## Release Pipeline

`release.yml` runs by `workflow_dispatch` with three inputs: `version` (SemVer without `v`), `prerelease` (boolean) and `summary`. Its jobs are:

1. **Prepare release.** Checks that the run is on `main` and the version is allowed, rejects a tag that already exists locally or on origin, and computes the tag, short SHA, release date and image tags.
2. **Backend validation** and **Web app validation**, which run in parallel. The backend job restores, builds and tests `DomusMind.slnx` in Release. The web app job runs `npm ci` and `npm run build`.
3. **Create tag.** Creates and pushes an annotated `v<version>` tag.
4. **Build and push image.** Generates `src/web/app/src/generated/version.ts` (`APP_VERSION`, `APP_RELEASE_DATE`, `APP_COMMIT_SHA`, `APP_IS_PRERELEASE`) and `release-manifest.json`, then builds and pushes the immutable tags with OCI `source`, `revision` and `version` labels.
5. **Publish GitHub Release.** Validates the compose file with `docker compose config`, then publishes the release with generated notes and the summary. The release assets are `deploy/docker-compose.yml`, `deploy/.env.example`, `deploy/README.md` and `release-manifest.json`.

A release can be traced through the git tag, the GitHub Release, the immutable image tags, the OCI labels and the in-app version metadata. The web app's Settings, About screen shows the version metadata. The backend has no version endpoint and logs no version at startup.

## CI Workflows

| Workflow | Job (check context) | What it validates |
| --- | --- | --- |
| [`backend-ci.yml`](../.github/workflows/backend-ci.yml) | `backend build` | Restore, build, test `DomusMind.slnx`; publishes the API as a run artifact |
| [`development-bff-e2e.yml`](../.github/workflows/development-bff-e2e.yml) | `Aspire, Keycloak and Vite browser security journey` | Runs the disposable two-identity OIDC BFF browser journey through Aspire and the Vite proxy, including forwarded Origin, CSRF, logout revocation and session expiry |
| [`image-postgres-smoke.yml`](../.github/workflows/image-postgres-smoke.yml) | `image postgres smoke` | Builds the release Dockerfile and starts the image with a disposable PostgreSQL database; verifies startup migrations, first-run setup status, API ping and the built SPA, then removes the containers, network and volumes |
| [`webapp-ci.yml`](../.github/workflows/webapp-ci.yml) | `webapp build` | `npm ci`, lint, test, build in `src/web/app` |
| [`public-site-ci.yml`](../.github/workflows/public-site-ci.yml) | `Validate Astro site` | Lint, Astro check and build in `src/web/public`; pull requests only |
| [`codeql.yml`](../.github/workflows/codeql.yml) | `Analyze (csharp)`, `Analyze (javascript-typescript)` | CodeQL; also weekly on schedule |
| [`product-ci.yml`](../.github/workflows/product-ci.yml) | `product model` | `prodshape validate`, `prodshape integration update --check` and `prodshape citations verify --provider openspec`, pinned to `@prodshape/cli@0.22.0` |
| [`product-snapshot.yml`](../.github/workflows/product-snapshot.yml) | `product snapshot` | Builds the Product Snapshot (`prodshape graph --format html`) and keeps it for 30 days as the run artifact `domusmind-product-snapshot.html`, linked from the run summary. It runs on pushes to `main` that touch the model and on pull requests that do |
| [`docs-ci.yml`](../.github/workflows/docs-ci.yml) | `architecture docs` | Checks the arc42 frontmatter contract (`scripts/check-arc42-frontmatter.mjs`), runs markdownlint over `docs/architecture` and `docs/adr`, and runs `prodshape citations verify docs/architecture`. Unresolved or tampered citations fail the job. Stale citations only warn |
| [`engineering-site.yml`](../.github/workflows/engineering-site.yml) | `Browser journeys`, `Build Engineering site`, `Publish Engineering site` | Runs generated-data browser coverage against the composed API, web app and PostgreSQL; publishes nightly reports and bounded evidence alongside the landing page, arc42/ADR and ProductShape snapshots. The non-cancelling publisher restores the current site manifest before updating it; see [`docs/engineering-site.md`](engineering-site.md) and [`docs/engineering-e2e.md`](engineering-e2e.md) |

[`public-site-cd.yml`](../.github/workflows/public-site-cd.yml) deploys the public site to Azure Static Web Apps on pushes to `main` that touch `src/web/public`. In short, CI validates, `docker-edge.yml` publishes edge images, and `release.yml` is the only publisher of release images.

The image smoke workflow runs on pull requests touching the backend, web app, Docker Compose installation, or the smoke-test workflow itself, and on matching pushes to `main`. It builds from the same [`DomusMind.Api/Dockerfile`](../src/backend/DomusMind.Api/Dockerfile) used by edge and release images. Its credentials and database are generated per run; it never connects to a developer or production database and does not publish the image.

### Required checks and the `changes` gate

Each gated CI workflow above (backend, webapp, public site, CodeQL, product model, product snapshot and architecture docs) starts with a small `changes` job, and its real job is gated on that job's output:

```yaml
jobs:
  changes:      # asks the API which files the pull request touches
  build:
    needs: changes
    if: needs.changes.outputs.relevant == 'true'
```

This looks redundant next to a `paths:` filter, but it is not. A path-filtered workflow reports **no check run at all**, and a required status check that never reports leaves the pull request blocked forever, so a docs-only change could never go green. A job skipped by an `if:` condition does report a check run, with conclusion `skipped`, and that **satisfies** the requirement.

So the `pull_request` triggers carry no `paths:` filter, and the `changes` job reproduces the filter instead. `push` triggers keep their filters, since required checks do not apply there.

Preserve these three consequences:

- Do not remove the `changes` jobs, and do not add `paths:` back to a `pull_request` trigger. Either one re-blocks unrelated pull requests.
- Job display names are the required check contexts, so renaming a job breaks branch protection until the contexts are updated to match. The backend and web app jobs are deliberately named `backend build` and `webapp build`. Both were previously called `build` and collided on a single context.
- **Do not turn a gated job back into a matrix.** A skipped matrix job reports a single check run named after the base job, with no matrix expansion, so the per-leg contexts never report and a required check on them can never be satisfied. This is why CodeQL runs as two jobs, `Analyze (csharp)` and `Analyze (javascript-typescript)`, rather than one matrix over `language`.

Current required contexts on `main`: `backend build`, `webapp build`, `Validate Astro site`, `Analyze (csharp)`, `Analyze (javascript-typescript)`. `product model`, `product snapshot` and `architecture docs` follow the same gate but are not required yet.

`required_approving_review_count` is `0`. Pull requests are still required, but GitHub forbids approving your own, so on a solo-maintained repository any non-zero count makes every pull request unmergeable without an admin bypass.

The release workflow repeats the backend and web app validation steps inline rather than calling a reusable workflow.

## Local Development and Aspire Telemetry

Run the AppHost from the repository root:

```bash
aspire run --apphost src/backend/DomusMind.AppHost/DomusMind.AppHost.csproj
```

The default `Ollama` provider starts PostgreSQL, the API, the Vite web app and the local model in Aspire. The Dashboard exposes resource health, traces, metrics and logs; the Aspire Server MCP can inspect those signals while the AppHost is running.

To use an OpenAI-compatible interpreter, keep its endpoint, model and API key in AppHost user secrets, then rerun the AppHost:

```bash
dotnet user-secrets --project src/backend/DomusMind.AppHost set "Inbox:Interpreter:Provider" "OpenAICompatible"
dotnet user-secrets --project src/backend/DomusMind.AppHost set "Inbox:Interpreter:Endpoint" "https://provider.example/v1/chat/completions"
dotnet user-secrets --project src/backend/DomusMind.AppHost set "Inbox:Interpreter:Model" "model-name"
dotnet user-secrets --project src/backend/DomusMind.AppHost set "Inbox:Interpreter:ApiKey" "your-api-key"
```

The external provider mode does not start Ollama. Inbox displays whether captured text leaves the deployment. Do not put provider credentials in committed settings files.

## Installation Configuration

All installation-specific values live in a host-side `.env` next to the compose file and are never committed. Start from [`deploy/.env.example`](../deploy/.env.example).

| Variable | Purpose |
| --- | --- |
| `IMAGE_REGISTRY`, `IMAGE_OWNER` | Where the image is pulled from (default `ghcr.io/juangcarmona`) |
| `VERSION` | Image tag to run. Pin an exact release version |
| `DB_USER`, `DB_PASSWORD` | PostgreSQL credentials |
| `JWT_SECRET` | Signing key of at least 32 characters, generated per installation (`openssl rand -hex 32`) |
| `JWT_ISSUER`, `JWT_AUDIENCE` | Token issuer and audience (default `domusmind`; the audience defaults to the issuer) |
| `APP_PORT` | Host port (default `24365`) |
| `REVERSE_PROXY_KNOWN_PROXIES`, `REVERSE_PROXY_KNOWN_IP_NETWORKS` | Optional comma-separated trusted proxy IPs or CIDR networks, as seen by the app container |
| `REVERSE_PROXY_ALLOWED_HOSTS` | Optional comma-separated allowlist for forwarded host names |
| `SWAGGER_ENABLED` | Whether to expose Swagger in Production (default `false`) |
| `BOOTSTRAP_ADMIN_EMAIL`, `BOOTSTRAP_ADMIN_PASSWORD` | Optional headless bootstrap only; disable it after first run |
| `MICROSOFT_GRAPH_CLIENT_ID`, `MICROSOFT_GRAPH_CLIENT_SECRET`, `MICROSOFT_GRAPH_TOKEN_ENCRYPTION_KEY`, `MICROSOFT_GRAPH_REDIRECT_URI` | Optional Outlook connection. Set all values together; generate the key with `openssl rand -base64 32`, and register the exact HTTPS callback URI in the Microsoft Entra app |
| `MICROSOFT_GRAPH_TENANT_ID` | Optional tenant ID or `common` (default) for Outlook authorization |

When Outlook is enabled, the API fails before database migrations if its client credentials, exact callback URI or 32-byte Base64 encryption key are invalid. Keep the encryption key stable across restarts and back it up with the database. Changing it makes existing Outlook token caches unreadable and requires each affected member to reconnect.

The Outlook OAuth hardening migration clears token caches and cached access tokens created by older versions, which stored them without authenticated encryption. The connection records, selected calendars and imported entries remain. After upgrading, each member must authorize the same Microsoft account again; DomusMind replaces credentials on the existing connection and preserves its calendar data. This is a one-time reconnect requirement, not a database restore requirement.

## Update Flow

1. Read the release notes and the migration notes.
2. Back up the database (see below).
3. Update `.env` if the release requires it, and set `VERSION` to the target release.
4. Run `docker compose pull`, then `docker compose up -d`.

EF Core migrations run automatically when the application starts. Schema changes are additive by default, and a breaking schema change requires a MAJOR release with explicit upgrade notes.

## Backup and Restore

PostgreSQL holds all persistent state, in the `postgres_data` volume. Two approaches are supported:

- **Logical dump** (portable across PostgreSQL versions):

  ```bash
  docker compose exec -T postgres pg_dump -U "$DB_USER" domusmind > domusmind_$(date +%Y%m%d_%H%M%S).sql
  docker compose exec -T postgres psql -U "$DB_USER" -d domusmind < domusmind_YYYYMMDD_HHMMSS.sql
  ```

- **Volume archive**, taken with the stack stopped, as described in [`deploy/README.md`](../deploy/README.md#volumes).

## Operational Rules

- Pin exact versions in installations. `edge` is for testing only.
- Releases come only from `release.yml` on `main`. Release tags are never reused or moved.
- Every release has release notes, plus migration notes when it changes the schema.
- Back up before every upgrade.
- Keep the runtime to one application instance and one database. Inbox interpretation selects `Ollama` or `OpenAICompatible` through `Inbox:Interpreter:Provider`; Aspire is for development only.
- Secrets live only in the host `.env`, GitHub Actions secrets or local user secrets.
- Put an HTTPS reverse proxy in front of any installation that is reachable beyond the local network.
