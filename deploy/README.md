# DomusMind - Self-hosted Installation

DomusMind runs as a Docker Compose stack: **postgres** and **domusmind**.

## Prerequisites

- Docker Engine 24+ with the Compose plugin (`docker compose version`)
- A GitHub Container Registry token or access to the GHCR namespace where images are published

## Quick start

```bash
# 1. Download the release assets
#    (or unzip the release archive - docker-compose.yml and .env.example are included)

# 2. Create your .env file
cp .env.example .env

# 3. Edit .env and fill in at minimum:
#    IMAGE_OWNER  - GitHub username or org that published the release
#    DB_PASSWORD  - a strong random password for the database
#    JWT_SECRET   - at least 32 random characters (openssl rand -hex 32)
#    VERSION      - an exact stable release version without the leading v

# 4. Start the stack
docker compose up -d

# 5. Open the web UI and complete the first-run setup wizard
#    http://localhost:${APP_PORT}
#
#    The wizard is served at the root URL on first visit.
#    Create the initial administrator account through the UI.
#    The setup endpoint is permanently server-gated: it cannot be called twice.
````

## Headless / recovery bootstrap (optional)

If you cannot use the UI-driven setup wizard (scripted provisioning, CI, disaster recovery),
you can seed the initial admin via environment variables.

This path is **disabled by default** and becomes a **permanent no-op once the system is
initialized** (whether by the UI wizard or a previous bootstrap run). It does not override
or re-initialize an already-initialized system.

To enable for a single run:

```bash
# Uncomment BootstrapAdmin__ lines in docker-compose.yml:
#   BootstrapAdmin__Enabled:  true
#   BootstrapAdmin__Email:    ${BOOTSTRAP_ADMIN_EMAIL}
#   BootstrapAdmin__Password: ${BOOTSTRAP_ADMIN_PASSWORD}

# Set credentials in .env:
#   BOOTSTRAP_ADMIN_EMAIL=admin@example.com
#   BOOTSTRAP_ADMIN_PASSWORD=<strong-password>

docker compose up -d

# Disable the bootstrap block in docker-compose.yml when done.
# On all subsequent restarts the setting is ignored because the system
# is already marked as initialized.
```

## Authentication settings

`JWT_ISSUER` defaults to `domusmind`. `JWT_AUDIENCE` is optional and Compose
defaults it to the issuer. If you override either value, keep the API and web
app configured with the same issuer and audience.

## Outlook calendar integration (optional)

The Compose file passes these optional values to the API:
`MICROSOFT_GRAPH_CLIENT_ID`, `MICROSOFT_GRAPH_CLIENT_SECRET`,
`MICROSOFT_GRAPH_TOKEN_ENCRYPTION_KEY`, and `MICROSOFT_GRAPH_REDIRECT_URI`.
Set these four values to enable Outlook, register the exact redirect URI in the
Microsoft Entra app, and keep the Base64 encryption key stable across restarts.
`MICROSOFT_GRAPH_TENANT_ID` is optional and defaults to `common`. The example
environment file lists each setting and the callback URI shape.

## Image tags

The release workflow publishes these exact image tags:

| Tag | Meaning | Guidance |
| --- | --- | --- |
| `1.2.3` | Exact stable release version; the same image also has `v1.2.3` and `sha-<shortsha>` tags | Use an exact stable version for persistent household installations |
| `1.2.3-beta.1`, `1.2.3-rc.1` | Exact prerelease versions; also tagged `v<version>` and `sha-<shortsha>` | Testing only; not a stable installation target |
| `edge` | Latest successful image build from `main` | Short-lived testing only; moves on each build |

The project does not publish `latest`, `MAJOR.MINOR`, or `alpha` tags. Set
`VERSION` to the exact stable version from the release you intend to install;
do not leave the template placeholder or use `edge` for a persistent install.

## Updates

```bash
docker compose pull
docker compose up -d
```

Migrations run automatically on app startup. Back up your `postgres_data`
volume before upgrading.

## HTTP and reverse proxy

The app container listens on HTTP port 8080. Direct local access through
`http://localhost:${APP_PORT}` remains available without a proxy. For public
access, terminate TLS at your reverse proxy and forward `X-Forwarded-For`,
`X-Forwarded-Proto`, and `X-Forwarded-Host` to DomusMind.

Set `REVERSE_PROXY_KNOWN_PROXIES` to the proxy's source IP as seen by the app
container, or `REVERSE_PROXY_KNOWN_IP_NETWORKS` to a narrowly scoped CIDR. Do
not trust a network that includes untrusted clients. Set
`REVERSE_PROXY_ALLOWED_HOSTS` to the public host name when you want to restrict
forwarded host values. The proxy should redirect public HTTP traffic to HTTPS;
DomusMind uses the forwarded scheme to build correct redirects and URLs. Keep
the app port reachable only from the proxy and trusted local operators.

For Nginx, the HTTPS virtual host can pass the request metadata like this. Keep
the HTTP virtual host's redirect to HTTPS at the proxy:

```nginx
location / {
    proxy_pass http://127.0.0.1:24365;
    proxy_set_header Host $host;
    proxy_set_header X-Forwarded-For $remote_addr;
    proxy_set_header X-Forwarded-Proto $scheme;
    proxy_set_header X-Forwarded-Host $host;
}
```

Swagger UI is off by default in Production. Set `SWAGGER_ENABLED=true` only
when it is exposed to a trusted network. The app's Docker health status probes
`/api/system/ping`; `docker compose ps` reports that status. Startup logs report
the app version and commit when supplied by the published image.

## Volumes

| Volume          | Contents             |
| --------------- | -------------------- |
| `postgres_data` | All application data |

Back up with:

```bash
docker run --rm \
  -v domusmind_postgres_data:/data \
  -v $(pwd):/backup \
  alpine tar czf /backup/postgres_data_$(date +%Y%m%d).tar.gz -C /data .
```

## Troubleshooting

```bash
# View logs
docker compose logs -f domusmind

# Check configuration before starting
docker compose config
```
