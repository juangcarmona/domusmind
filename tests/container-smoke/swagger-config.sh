#!/usr/bin/env bash
set -Eeuo pipefail

base_url="${SMOKE_BASE_URL:-http://127.0.0.1:${SMOKE_PROXY_PORT:-18081}}"
status="$(curl --silent --show-error --max-time 10 --output /dev/null \
  --write-out '%{http_code}' \
  -H 'X-Forwarded-Proto: https' \
  -H 'X-Forwarded-Host: household.example.test' \
  "$base_url/swagger/index.html")"

test "$status" = 200 || {
  echo "Swagger__Enabled=true should expose the Swagger UI (HTTP $status)" >&2
  exit 1
}

echo "Swagger configuration smoke check passed."
