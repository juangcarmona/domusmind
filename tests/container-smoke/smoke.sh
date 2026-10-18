#!/usr/bin/env bash
set -Eeuo pipefail

base_url="${SMOKE_BASE_URL:-http://127.0.0.1:${SMOKE_APP_PORT:-18080}}"
setup_status_file="$(mktemp)"
html_file="$(mktemp)"
headers_file="$(mktemp)"
trap 'rm -f "$setup_status_file" "$html_file" "$headers_file"' EXIT

ready=false
for attempt in $(seq 1 60); do
  if curl --silent --fail --max-time 3 \
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
  "$base_url/api/system/ping" | jq --exit-status '.status == "ok"' >/dev/null

curl --silent --show-error --fail --max-time 10 \
  --dump-header "$headers_file" "$base_url/" --output "$html_file"
grep -qi '^content-type: text/html' "$headers_file"
grep -q '<div id="root"></div>' "$html_file"

echo "Image smoke checks passed: migrations/readiness, first-run setup status, API ping, and built SPA."
