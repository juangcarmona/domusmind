# Engineering site

The Engineering site is a static GitHub Pages publication, independent of the
Azure Static Web Apps deployment for [domusmind.org](https://domusmind.org/).
Its source content is generated from the repository on every relevant push to
`main` and from a manual workflow run.

## Published sections

- `/` is an engineering landing page using the public site's typography,
  lavender palette, brand mark and responsive spacing. It links to technical
  documentation and does not reuse marketing copy.
- `/architecture/` is a self-contained arc42 snapshot generated from
  `docs/architecture/` and the records in `docs/adr/`. Mermaid diagrams are
  rendered to inline SVG; the output has no runtime dependency on the source
  repository or external assets.
- `/model/` is generated with the pinned ProductShape CLI version used by
  `product-snapshot.yml`.
- `/e2e/` contains the browser journey report index, a stable latest URL, and
  bounded history. See [Browser end-to-end reports](engineering-e2e.md) for
  the selected journeys, evidence and retention policy.

`tools/static-site/static-site.mjs` restores files listed in the current
`site-manifest.json`, builds the landing page, and writes a new manifest for the
combined site. This lets later site sections, such as nightly browser reports,
be updated without replacing the other published sections. The workflow uses a
single non-cancelling publisher concurrency group; it needs only GitHub's Pages
deployment token and short-lived OIDC token, not a stored publishing secret.

## One-time GitHub setup

In repository **Settings → Pages**, select **GitHub Actions** as the build and
deployment source. The first successful run publishes the site at the base URL
provided by `actions/configure-pages`; all generated links honor that base path.
The publisher workflow can also be run from **Actions → engineering-site → Run
workflow** to initialize or refresh the publication.

The `engineering-site` workflow validates utility tests and generates both
snapshots during pull requests. It also runs the selected browser journey in an
ephemeral composed environment; only pushes to `main`, nightly runs and manual
runs deploy.
The public marketing site's Azure workflow and deployment credential are not
used by this publisher.
