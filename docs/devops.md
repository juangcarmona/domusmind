# DomusMind DevOps

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
| [`webapp-ci.yml`](../.github/workflows/webapp-ci.yml) | `webapp build` | `npm ci`, lint, test, build in `src/web/app` |
| [`public-site-ci.yml`](../.github/workflows/public-site-ci.yml) | `Validate Astro site` | Lint, Astro check and build in `src/web/public`; pull requests only |
| [`codeql.yml`](../.github/workflows/codeql.yml) | `Analyze (csharp)`, `Analyze (javascript-typescript)` | CodeQL; also weekly on schedule |
| [`product-ci.yml`](../.github/workflows/product-ci.yml) | `product model` | `prodshape validate`, `prodshape integration update --check` and `prodshape citations verify --provider openspec`, pinned to `@prodshape/cli@0.22.0` |
| [`product-snapshot.yml`](../.github/workflows/product-snapshot.yml) | `product snapshot` | Builds the Product Snapshot (`prodshape graph --format html`) and keeps it for 30 days as the run artifact `domusmind-product-snapshot.html`, linked from the run summary. It runs on pushes to `main` that touch the model and on pull requests that do |
| [`docs-ci.yml`](../.github/workflows/docs-ci.yml) | `architecture docs` | Checks the arc42 frontmatter contract (`scripts/check-arc42-frontmatter.mjs`), runs markdownlint over `docs/architecture` and `docs/adr`, and runs `prodshape citations verify docs/architecture`. Unresolved or tampered citations fail the job. Stale citations only warn |
| [`mobile-ci.yml`](../.github/workflows/mobile-ci.yml) | `build` | Builds `src/mobile/DomusMind.Mobile`, a directory that does not exist in the repository. The workflow uses `paths:` filters and has no `changes` job |

[`public-site-cd.yml`](../.github/workflows/public-site-cd.yml) deploys the public site to Azure Static Web Apps on pushes to `main` that touch `src/web/public`. In short, CI validates, `docker-edge.yml` publishes edge images, and `release.yml` is the only publisher of release images.

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
| `BOOTSTRAP_ADMIN_EMAIL`, `BOOTSTRAP_ADMIN_PASSWORD` | Optional headless bootstrap only; disable it after first run |

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
- Keep the runtime to one application instance and one database. Aspire is for development only.
- Secrets live only in the host `.env`, GitHub Actions secrets or local user secrets.
- Put an HTTPS reverse proxy in front of any installation that is reachable beyond the local network.
