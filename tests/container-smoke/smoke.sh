#!/usr/bin/env bash
set -Eeuo pipefail

base_url="${SMOKE_BASE_URL:-http://127.0.0.1:${SMOKE_PROXY_PORT:-18081}}"
direct_url="${SMOKE_DIRECT_BASE_URL:-http://127.0.0.1:${SMOKE_APP_PORT:-18080}}"
setup_status_file="$(mktemp)"
html_file="$(mktemp)"
headers_file="$(mktemp)"
trap 'rm -f "$setup_status_file" "$html_file" "$headers_file"' EXIT

ready=false
for attempt in $(seq 1 60); do
  if curl --silent --fail --max-time 3 \
    -H 'X-Forwarded-Proto: https' \
    -H 'X-Forwarded-Host: household.example.test' \
    "$base_url/api/setup/status" --output "$setup_status_file"; then
    ready=true
    break
  fi
  sleep 2
done

if [ "$ready" != true ]; then
  echo "Setup status did not become ready at $base_url/api/setup/status" >&2
  exit 1
fi

jq --exit-status '.isInitialized == false' "$setup_status_file" >/dev/null || {
  echo "A clean PostgreSQL database should report isInitialized=false" >&2
  cat "$setup_status_file" >&2
  exit 1
}

curl --silent --show-error --fail --max-time 10 \
  -H 'X-Forwarded-Proto: https' \
  -H 'X-Forwarded-Host: household.example.test' \
  "$base_url/api/system/ping" | jq --exit-status '.status == "ok"' >/dev/null

curl --silent --show-error --fail --max-time 10 \
  -H 'X-Forwarded-Proto: https' \
  -H 'X-Forwarded-Host: household.example.test' \
  --dump-header "$headers_file" "$base_url/" --output "$html_file"
grep -qi '^content-type: text/html' "$headers_file"
grep -q '<div id="root"></div>' "$html_file"

curl --silent --show-error --max-time 10 \
  --output /dev/null --dump-header "$headers_file" \
  -H 'X-Forwarded-Proto: http' \
  -H 'X-Forwarded-Host: household.example.test' \
  "$base_url/api/system/ping"
grep -qi '^location: https://household.example.test/api/system/ping' "$headers_file" || {
  echo "The proxy should redirect public HTTP requests to its HTTPS URL" >&2
  cat "$headers_file" >&2
  exit 1
}

curl --silent --show-error --fail --max-time 10 \
  -H 'X-Forwarded-Proto: https' \
  -H 'X-Forwarded-Host: attacker.example.test' \
  "$direct_url/api/system/ping" | jq --exit-status '.status == "ok"' >/dev/null || {
  echo "Untrusted forwarded headers must not change direct request host or scheme" >&2
  exit 1
}

app_id="$(docker compose --file tests/container-smoke/compose.yml ps --quiet app)"
health="$(docker inspect --format '{{.State.Health.Status}}' "$app_id")"
test "$health" = healthy || {
  echo "Expected a healthy app container, found '$health'" >&2
  exit 1
}
app_logs="$(docker compose --file tests/container-smoke/compose.yml logs app)"
grep -Eq 'DomusMind version (local|[0-9])' <<<"$app_logs" || {
    echo "The application startup logs should identify its version" >&2
    exit 1
  }

curl --silent --show-error --max-time 10 --output /dev/null \
  --write-out '%{http_code}' \
  -H 'X-Forwarded-Proto: https' \
  -H 'X-Forwarded-Host: household.example.test' \
  "$base_url/swagger/index.html" | grep -qx 404 || {
    echo "Swagger should be disabled by default in Production" >&2
    exit 1
  }

echo "Image smoke checks passed: migrations, setup status, proxy URLs and trust, API, SPA, health, version log, and default Swagger policy."
