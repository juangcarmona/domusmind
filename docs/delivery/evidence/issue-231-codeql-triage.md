# Issue #231 CodeQL alert triage

- Analysis: [CodeQL analysis 1993001812](https://github.com/juangcarmona/domusmind/code-scanning/analyses/1993001812)
- Application/test head: `53623d6c5703650baaf84f6ef1f2b7d47f729915`
- Rule: `cs/web/missing-token-validation`
- Alerts reviewed: 64 (alerts 6–69)
- Disposition: false positive for this repository's authentication model

## Decision

The alerts are the same CSRF query reported against existing API controllers and the MCP OAuth controller. The REST API controllers authenticate API requests with bearer tokens; they do not authenticate the affected state-changing calls with browser cookies, so a cross-site browser request cannot reuse a session cookie to authorize those actions. The OAuth token endpoint is a back-channel authorization-code/refresh-token exchange and likewise does not use the browser cookie.

The browser-facing MCP OAuth actions are handled separately:

- `McpOAuthController.Authorize` validates the antiforgery request before processing the POST consent form.
- `McpOAuthController.Login` validates the antiforgery request before establishing the OAuth browser session.
- `McpOAuthController.RevokeConsent` validates the antiforgery request before revoking consent.
- `McpWriteApprovalsController.Approve` and `Cancel` use `[ValidateAntiForgeryToken]`, with the API host registering the MVC filter services required by that attribute.

The CodeQL query does not recognize the repository's bearer-token boundary or the explicit `IAntiforgery.ValidateRequestAsync` calls and therefore reports these covered/irrelevant endpoints as missing validation. The 64 alerts are dismissed as false positives with this disposition recorded in GitHub. This is separate from inherited package advisories and does not suppress other CodeQL rules.

## Verification

- The approval endpoint rejects MCP bearer credentials because it accepts only the first-party cookie scheme.
- A cookie-authenticated approval request without an antiforgery token returns `400` before the coordinator is called.
- Hosted CodeQL passed for C# and JavaScript after the implementation correction; the open-alert state is checked again after this disposition.
