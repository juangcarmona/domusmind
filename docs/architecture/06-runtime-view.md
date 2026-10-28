---
title: Runtime View
arc42-section: "06"
description: Architecturally significant runtime scenarios of DomusMind and how its building blocks cooperate in them, including where the running code departs from the intended event-driven collaboration.
---

# Runtime View

## Development browser session pilot

With `Bff:Enabled` in development, the SPA opens `/api/session/login`; the API host challenges Keycloak through Authorization Code with PKCE. The callback accepts only a configured test subject whose verified email matches an active local `AuthUser`, then issues a Secure, HttpOnly cookie referencing a server-side authentication ticket. The in-memory ticket store revokes that ticket on logout and is limited to one development API process. The SPA calls the same-origin controllers without bearer tokens. Unsafe `/api` requests must carry the CSRF request token and matching Origin; production still uses the local JWT flow described below.

Participants are the building blocks of [section 05](05-building-block-view.md). Each scenario describes the code as it runs today; where it differs from the intended design the difference is stated and tracked in [section 11](11-risks-and-technical-debt.md).

## Command through the internal dispatcher

Every state change follows the same path through the internal mediator ([ADR-0001](../adr/0001-use-an-internal-application-mediator.md)). Scheduling a plan is representative.

```mermaid
sequenceDiagram
    participant Web as Web app
    participant Ctl as Api controller
    participant CD as CommandDispatcher
    participant H as ScheduleEvent handler
    participant Auth as FamilyAuthorizationService
    participant Agg as CalendarEvent aggregate
    participant ELW as EventLogWriter
    participant DD as DomainEventDispatcher
    participant EH as DomainEventHandler
    participant DB as DomusMind database

    Web->>Ctl: POST with bearer token
    Ctl->>CD: Dispatch(ScheduleEventCommand incl. current user id)
    CD->>H: resolve ICommandHandler from DI, invoke Handle
    H->>Auth: CanAccessFamilyAsync(user, family)
    Auth->>DB: read user-to-household access grant
    H->>Agg: Create(...) raises EventScheduled
    H->>H: stage aggregate in DbContext
    H->>ELW: WriteAsync([aggregate])
    ELW->>DB: Begin transaction, save aggregate and event-log rows
    ELW->>DD: Dispatch the persisted domain events
    opt matching event handler registered
        DD->>EH: Resolve IDomainEventHandler from DI
        EH->>DB: Save reaction aggregate and its event-log rows
    end
    ELW->>DB: Commit transaction after every reaction succeeds
    H->>Agg: ClearDomainEvents
    H-->>Ctl: Contracts response
    Ctl-->>Web: 2xx JSON
```

### Notable Interactions

- The controller adds only the authenticated user's id to the command; the handler decides access to the household and enforces it itself. There is no dispatcher pipeline: validation, authorization and mapping happen inside the handler.
- The event-log writer saves aggregate state and event rows before dispatching the events through the internal dispatcher. In PostgreSQL it holds the transaction open until all synchronous reactions and their event-log writes succeed; a failure rolls back the originating aggregate, event rows and reaction aggregates together.
- Handlers clear their buffered events after the writer returns. The dispatcher runs registered handlers in the same request and DbContext scope; it does not add a broker or background worker.

Queries follow the same shape through `QueryDispatcher` to a query handler that reads with `AsNoTracking()` projections and returns a `Contracts` model; they write nothing.

## Shared-list updates across open sessions

The API publishes an empty invalidation notice only after a Lists command has committed. A subscriber reloads the list through the existing authenticated detail query, so the stream does not carry household data.

```mermaid
sequenceDiagram
    participant A as Member A browser
    participant C as ListsController
    participant H as Lists command handler
    participant DB as DomusMind database
    participant N as ListChangeNotifier
    participant S as Member B SSE stream
    participant B as Member B browser

    B->>C: GET /api/shared-lists/{id}/events with bearer token
    C->>H: authorize through GetListDetail query
    H->>DB: check list access and household membership
    C-->>B: 200 text/event-stream with refresh event
    B->>C: GET /api/shared-lists/{id} with bearer token
    A->>C: POST/PATCH list mutation with bearer token
    C->>H: Dispatch list command
    H->>DB: authorize, update SharedList, commit event log
    C->>N: publish list id after command succeeds
    N-->>S: empty refresh event for this list
    S-->>B: forward refresh event
    B->>C: GET /api/shared-lists/{id} with bearer token
    C->>H: authorize through GetListDetail query
    H->>DB: load current list detail
    C-->>B: current detail response
```

`ListChangeNotifier` is in-memory and supports the one-application-instance self-hosted topology. The controller rechecks subscription access on its keep-alive interval, while every mutation handler performs its own household authorization. On reconnect, the stream sends another refresh event so the browser reloads current state. The product does not define a latency target; the browser journey checks eventual visibility without a manual refresh.

## Capture and interpret an Inbox note

The authenticated capture request stores the raw note in the Inbox aggregate first. A separate command invokes the configured interpreter so an unavailable provider never loses the captured note. The provider-neutral application port returns structured proposals; the Inbox handler persists them as pending. Infrastructure selects Ollama or an OpenAI-compatible endpoint from configuration, and the Inbox tells the member when text is sent outside the deployment.

```mermaid
sequenceDiagram
    participant Web as Web app
    participant C as InboxController
    participant H as Inbox handlers
    participant Auth as FamilyAuthorizationService
    participant DB as DomusMind database
    participant O as Configured interpreter
    participant EL as EventLogWriter
    Web->>C: POST /api/inbox with text and family id
    C->>H: Dispatch(CaptureInboxItemCommand, current user)
    H->>Auth: Check household access
    H->>DB: Save raw Inbox item and capture event
    C-->>Web: 201 captured item
    Web->>C: POST /api/inbox/{id}/interpret
    C->>H: Dispatch(InterpretInboxItemCommand, current user)
    H->>Auth: Check household access
    H->>DB: Read raw note
    H->>O: Structured interpretation request
    O-->>H: Proposal fields
    H->>EL: Save pending proposals and interpretation event
    H-->>C: Inbox response model
    C-->>Web: 200 proposals for review
```

The member's approval command changes one Inbox aggregate and emits `InboxProposalApproved`. Its synchronous domain-event reaction dispatches the normal Calendar or Tasks command, then marks the proposal applied. The owning area handler repeats household authorization and input validation. The event writer keeps all aggregate and event-log writes in one PostgreSQL transaction, so a failed action leaves the proposal pending and creates no Plan or Task. Rejected proposals update only Inbox state. <!-- pdac:cite id="BR-INBOX-REVIEW-AND-EXECUTION" digest="sha256:7c54cc5879964502757faea971dacd03b841305ae6166d64072cde41aea1303f" -->

## Household conversation and explicit assistant request

The Chats controller reads the authenticated user and dispatches commands and queries. Handlers resolve the household member, enforce thread participation, and persist each message through the event-log writer. For an explicit `@dm` request, the application gathers information through the invoking member's authorized read path and asks the household's ordered provider route. Only retryable availability or required-capability failures advance to the next target. The assistant response records the handling target and whether fallback occurred; route details and saved credentials are managed through a household-scoped application slice, with credentials protected by ASP.NET Core Data Protection.

For a complete action request in a household thread, the assistant message raises `AssistantProposalRequested`. The Inbox-owned reaction stores an unreviewed proposal linked to the originating Chat message. It does not call a Calendar or Lists command. A private-thread request never raises this event. On explicit approval, Inbox dispatches the normal command owned by Calendar or Lists, then marks its proposal applied. The Chat message, event and Inbox reaction participate in the event writer's transaction.

<!-- pdac:cite id="FR-CHAT-ASK-ASSISTANT" digest="sha256:af9ab2889fe4bb5b9168f43d0725874432354e723000b96ae0d732fd8ffc597b" -->
<!-- pdac:cite id="FR-INBOX-INTERPRET" digest="sha256:f1a488d642604dbe43c9e465683fa2602b7f027d5618c4c4d3ac7e73cc29e8be" -->
<!-- pdac:cite id="FR-ASSISTANT-CONFIGURE-PROVIDERS" digest="sha256:6f46b897ec17ba34fa6027cdfa95a01a41d176d06edb4e69daced913c924bdc8" -->
<!-- pdac:cite id="QR-ASSISTANT-PROVIDER-PRIVACY" digest="sha256:769c170bfea0d620610f7d331581c0c5c7685f962bcaba4060442dba5a2eb4b2" -->

```mermaid
sequenceDiagram
    participant Web as Web app
    participant C as ChatsController
    participant H as Chat handlers
    participant Auth as FamilyAuthorizationService
    participant DB as DomusMind database
    participant Q as Existing authorized read query
    participant R as Household provider route
    participant AI as Hosted or local target
    participant IR as Inbox proposal reaction
    participant EL as EventLogWriter

    Web->>C: POST /api/chats/{threadId}/messages with bearer token
    C->>H: Dispatch(PostChatMessageCommand, current user)
    H->>Auth: Check household access and thread participation
    H->>DB: Persist member message and event-log row
    alt Text begins with @dm
        C->>H: Dispatch(AnswerAssistantQuestionCommand)
        H->>Auth: Recheck household access and participation
        H->>Q: Dispatch read-only query for classified topic
        Q->>DB: Read authorized household data
        H->>R: Resolve ordered targets for household
        R->>AI: Question plus authorized context
        alt Retryable provider failure or unsupported capability
            AI-->>R: Retryable failure
            R->>AI: Try next configured target
        end
        AI-->>R: Answer and optional structured proposal
        R-->>H: Completion, target and fallback status
        H->>EL: Persist assistant message linked to question
        opt Complete action in a household thread
            EL->>IR: Dispatch AssistantProposalRequested
            IR->>DB: Save Inbox item linked to source Chat message
        end
    end
    C-->>Web: 201 member message and optional assistant response
```

The member message is persisted before calling the provider, so an unavailable assistant does not lose the conversation text. The API indicates that the assistant is unavailable while retaining the member's message. Thread list and message queries return data only to participants. Chat provider metadata and the Inbox source-message link preserve the origin needed to review which target handled the request and whether fallback occurred.

### Failure Behavior

Handlers throw a module-specific exception (for example `CalendarException` with an error code); controllers translate those into 400, 403 or 404 responses. A missing handler registration raises `HandlerResolutionException`, which the global exception handler maps to a 500 response with a `handler_resolution_failed` code; any other unhandled exception becomes a generic 500 with a trace id.

## Cross-module reaction: shopping list from a meal plan

Meal Planning emits `ShoppingListRequested` after checking household access and confirming that the plan contains a recipe slot. The Lists module owns the reaction and creates the `SharedList` from the event ([ADR-0008](../adr/0008-collaborate-across-modules-through-persisted-domain-events.md)).

```mermaid
sequenceDiagram
    participant Ctl as MealPlansController
    participant H as RequestShoppingList handler
    participant MP as MealPlan aggregate
    participant DD as DomainEventDispatcher
    participant LH as Lists reaction handler
    participant SL as SharedList aggregate
    participant ELW as EventLogWriter
    participant DB as DomusMind database

    Ctl->>H: Dispatch(RequestShoppingListCommand)
    H->>DB: load MealPlan with slots and authorize family
    H->>MP: Record list id, derivation version and ShoppingListRequested
    H->>ELW: WriteAsync([MealPlan])
    ELW->>DB: Begin transaction and save MealPlan plus event_log
    ELW->>DD: Dispatch ShoppingListRequested
    DD->>LH: invoke Lists-owned handler
    LH->>DB: load MealPlan slots and recipes with ingredients
    LH->>LH: consolidate ingredients by name and unit
    LH->>SL: Create SharedList with event's stable list id and add items
    LH->>ELW: WriteAsync([SharedList])
    ELW->>DB: save SharedList and its event_log rows in same transaction
    ELW->>DB: commit after reaction succeeds
    H-->>Ctl: plan id, list id, item count
```

### Notable Interactions

- The new list is a regular `SharedList` from the moment it exists and is linked back to the meal plan through its untyped link fields. Meal Planning stores the requested list id and derivation version; Lists owns creation and item contents.
  <!-- pdac:cite id="BR-MEALS-SHOPPING-LIST-NEW-EACH-TIME" digest="sha256:66bbb87ee56438054657395a8491fb49c3b1c8b4ded3b5451c8c3933b50f08e1" -->
- A repeated delivery uses the same list id from the event. The Lists handler finds the existing list and returns without creating duplicate rows or events.

### Failure Behavior

The originating MealPlan, `ShoppingListRequested` row, new `SharedList`, and its event rows commit together in PostgreSQL. If the Lists reaction fails, the transaction rolls back and the command can be retried without a partial list or a bumped derivation version. A plan with no recipe slots is rejected before anything is written.

## Calendar subscription access

Subscription management uses authenticated REST commands and queries. A feed request carries its bearer secret in the URL, resolves only a stored token hash, rejects revoked or expired subscriptions, projects scoped Plans into ICS, and records successful access metadata in the subscription aggregate and event log. The route uses a parameterized template so request telemetry identifies the endpoint without needing the token value.
<!-- pdac:cite id="FR-CALENDAR-MANAGE-SUBSCRIPTIONS" digest="sha256:be31d2356bcd4166ed05806ff0acb55dd0fb3b610bc92c3857bd562b47b0d5b7" -->
<!-- pdac:cite id="FR-CALENDAR-SERVE-SUBSCRIPTION-FEED" digest="sha256:99590c81c57bf55ed7f7fbfb9fa0ae8f00e6559176ef6c9d7aa8b4ed043f7a6c" -->

```mermaid
sequenceDiagram
    participant O as Subscription owner
    participant W as Web app
    participant C as CalendarSubscriptionsController
    participant H as Calendar application handler
    participant D as PostgreSQL
    participant X as External calendar client

    O->>W: Create household or member subscription
    W->>C: POST /api/calendar/subscriptions (JWT)
    C->>H: Dispatch create command
    H->>D: Persist subscription and token hash, then append domain event
    H-->>W: Return private URL once
    X->>C: GET /calendar/subscriptions/{token}.ics
    C->>H: Dispatch feed query
    H->>D: Verify hash, load scoped Plans, record successful access
    H-->>X: Read-only ICS (title and timing)
```

The application never persists the raw URL. Listing is owner-scoped and returns access metadata only; revocation changes the subscription and is appended to the event log. Feed URLs are bearer credentials and must not be included in application logs.
<!-- pdac:cite id="BR-CALENDAR-SUBSCRIPTION-ACCESS" digest="sha256:f23e6df00c9fa568487a80d9a9aabcbb42cad4892c2307a8925fe1ee4a786da3" -->
<!-- pdac:cite id="QR-CALENDAR-SUBSCRIPTION-SECURITY" digest="sha256:e1d9579e065244b2ee489c776fe6a3cf2ffd3c586666b7f2b9127101d64bd62f" -->

## Agenda read projection

The Agenda is assembled at read time; it has no stored projection and no event subscribers. The web app's Agenda (household and member scope, day, week and month) is fed by one query, the weekly grid, which reads the aggregates of several modules directly through `IDomusMindDbContext`.

```mermaid
flowchart LR
    Web["Web app Agenda"] -->|GET weekly-grid| Q["GetWeeklyGrid query handler<br/>Features/Family"]
    Q --> F[(Family members)]
    Q --> C[(CalendarEvent)]
    Q --> T[(HouseholdTask, Routine)]
    Q --> L[(SharedList items with temporal fields)]
    Q --> R[(ResponsibilityDomain names)]
    Q --> X[(ExternalCalendarConnection and entries)]
    Q -->|week grid per member and household| Web
```

### Notable Interactions

- Each source module stays the owner of its entries; the projection is read-only and the web app edits an entry by navigating to its owning surface, which calls that module's commands.
  <!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->
- Imported external entries are added only to the row of the member who owns the connection.
  <!-- pdac:cite id="BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY" digest="sha256:b1dcb8fae830badbc584fe2a2e8b3ec117053470d9123391b2fa0d69f6bc9e69" -->
- Recurring routines and repeating list items are expanded into occurrences in the query (`Application/Temporal`), not stored as occurrences.
- Meal Planning is not part of the grid. A separate meal-plan agenda query exists (`Features/MealPlanning/GetMealPlansForAgenda`) and a member agenda query exists (`Features/Calendar/GetMemberAgenda`), but the web app calls neither, so meals do not appear in the Agenda today.
  <!-- pdac:cite id="FR-MEALS-AGENDA-PROJECTION" digest="sha256:14542a51f00845c388cd4e159f50674eca6d349656eacac4935fa63b064cd244" -->

## External calendar sync with Outlook

External calendars are ingested one way, from Microsoft Graph into Calendar-owned integration records, with delegated per-member access ([ADR-0003](../adr/0003-use-delegated-graph-auth-for-outlook-calendar-ingestion.md)).

<!-- pdac:cite id="CON-CALENDAR-OUTLOOK-PULL-ONLY" digest="sha256:a631bcb19c09716b7cbee7b25563b55eec7ca4287de67c84cb1081144d29deb9" -->

Two triggers converge on the same command: a member's manual sync from Settings, and the `ExternalCalendarRefreshWorker` hosted service, which wakes every 300 seconds and selects up to 10 due connections (scheduled refresh enabled, not disconnected or auth-expired, next sync time reached, no live lease). The worker selects the most overdue connections first; connections without a next-sync timestamp come first, and connection id breaks ties. It dispatches the sync for each with a small random delay. Each connection's next sync is scheduled from its own interval, 60 minutes by default.

<!-- pdac:cite id="FR-CALENDAR-SYNC-EXTERNAL-CALENDAR" digest="sha256:2e4013f14b0b0d3092f017991253bd2441931459bd1db5b24d0bc5a6c00dc423" -->
<!-- pdac:cite id="FR-CALENDAR-BACKGROUND-REFRESH" digest="sha256:4a820cb4807164b225e417fbb59c0b4cba8a5b188bde26f1821e62a258b2aeb4" -->

```mermaid
sequenceDiagram
    participant Trig as Settings (manual) or RefreshWorker (scheduled)
    participant H as SyncExternalCalendarConnection handler
    participant Lease as Sync lease service
    participant Conn as ExternalCalendarConnection aggregate
    participant GA as Graph auth service
    participant GC as Graph calendar client
    participant DB as DomusMind database

    Trig->>H: Dispatch(SyncExternalCalendarConnectionCommand)
    H->>Lease: TryAcquire (10-minute lease on the connection)
    H->>Conn: MarkSyncing, SaveChanges
    H->>GA: GetAccessToken (cached token or silent MSAL refresh from encrypted cache)
    loop each selected feed
        H->>GC: initial window load, or delta from stored delta token
        H->>DB: upsert or delete external entries, record delta token, SaveChanges
    end
    H->>Conn: RecordSyncSuccess, PartialFailure or Failure
    H->>DB: SaveChanges, write connection events to event log
    H->>Lease: Release
```

### Notable Interactions

- The lease stored on the connection prevents a connection from syncing twice at once, whether the second trigger is manual or scheduled; a manual sync during a running one is rejected as in progress.
  <!-- pdac:cite id="BR-CALENDAR-NO-CONCURRENT-SYNC" digest="sha256:85649c4ba718266f5043e7a39f97fef18f3cdaec34b67e91bdb207cedb87974b" -->
- The first sync of a feed loads the sync window computed from the connection's horizon; later syncs use the Graph delta token. Changing the horizon invalidates every feed's delta state so the next run reloads the window.
- Imported entries are stored only as Calendar integration records and never create native plans.
- The catch-up sync on sign-in or on opening a member's Agenda is not implemented: the API exposes a sync-all endpoint for a member, but the web app never calls it.

### Failure Behavior

- A feed that fails is logged and counted; the other feeds continue, and the connection records partial failure or, when all feeds fail, failure. In the worker, one connection's failure is logged and the batch continues.
- When no access token can be obtained the connection is marked auth-expired, which excludes it from scheduled refresh until the member reconnects.
- The lease is always released in a `finally` path; if the process dies, the lease expires after 10 minutes and the worker picks the connection up again.
- There is no recovery path for a delta token the provider rejects: the feed fails on every run until the horizon changes or the member reconnects.

## Authentication and sign-in

Authentication is local to the API and separate from household membership ([ADR-0002](../adr/0002-keep-authentication-local-and-separate-from-member-identity.md)). The mechanisms (hashing, token formats, lifetimes, storage) are owned by [section 08](08-crosscutting-concepts.md).

```mermaid
sequenceDiagram
    participant Web as Web app
    participant AC as AuthController
    participant H as Login handler
    participant Users as AuthUserRepository
    participant RT as RefreshTokenStore
    participant DB as DomusMind database

    Web->>AC: POST /api/auth/login (email, password)
    AC->>H: Dispatch(LoginCommand)
    H->>Users: find by normalized email, verify password hash
    H->>H: reject if disabled, issue JWT access token
    H->>RT: create refresh token (stored hashed)
    RT->>DB: insert
    H-->>Web: access token, refresh token, must-change-password flag
    Web->>AC: GET /api/auth/me (bearer)
    Note over Web: tokens kept in browser storage
    Web->>AC: later requests carry the bearer token
    AC->>AC: JWT bearer middleware validates, CurrentUserAccessor exposes user id
```

### Notable Interactions

- The authenticated principal is an auth user, not a household member. Which households a user may act in is resolved per command or query from the user-to-household access table by `FamilyAuthorizationService`; a member is linked to an auth user only through an optional account id on the member.
  <!-- pdac:cite id="TERM-MEMBER-ACCESS" digest="sha256:b7985c2011d508e2f15b31f8079b9dd54cd5c3603870e83324c4892601f1593b" -->
- On load, the web app validates its stored access token with `/api/auth/me`; if that fails it exchanges the refresh token once, which rotates it (the old one is revoked), and otherwise clears the session. There is no refresh on a 401 during normal use.

### Failure Behavior

Wrong credentials and an unknown email return the same invalid-credentials error; a disabled account is refused after the password check. An invalid, expired or revoked refresh token returns 401 and the web app falls back to the sign-in screen.
