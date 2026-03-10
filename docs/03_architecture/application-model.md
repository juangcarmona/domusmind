# DomusMind — Application Model

## Purpose

This document defines the **application execution model** used by DomusMind.

The application layer translates system capabilities into domain behavior.

All interactions with the domain follow a **Command / Query model**.

---

# Core Concepts

## Commands

Commands represent **intent to change system state**.

Examples:

```

CreateFamily
AddMember
ScheduleEvent
CreateTask

```

Properties:

- mutate domain state
- handled by a single handler
- operate on a single aggregate
- may emit domain events

Contract:

```

ICommand<TResponse>

```

Handler:

```

ICommandHandler<TCommand, TResponse>

```

---

## Queries

Queries represent **read operations**.

Examples:

```

GetFamily
GetFamilyTimeline
GetMemberSchedule

```

Properties:

- do not modify domain state
- may read multiple aggregates
- optimized for read models

Contract:

```

IQuery<TResponse>

```

Handler:

```

IQueryHandler<TQuery, TResponse>

```

---

## Domain Events

Domain events represent **facts that occurred in the domain**.

Examples:

```

FamilyCreated
EventScheduled
TaskCompleted

```

Contract:

```

IDomainEvent

```

Handlers:

```

IDomainEventHandler<TEvent>

```

Events may trigger behavior in other modules.

---

# Dispatchers

Dispatchers route requests to handlers.

Interfaces:

```

ICommandDispatcher
IQueryDispatcher
IDomainEventDispatcher

```

Responsibilities:

- resolve handlers
- execute handlers
- publish domain events

Dispatchers use the system dependency injection container.

---

# Execution Flow

Typical command execution:

```

Client
↓
API Endpoint
↓
Command Dispatcher
↓
Command Handler
↓
Aggregate
↓
Domain Events
↓
Event Handlers

```

---

# Validation

Validation occurs before command execution.

Typical flow:

```

Command
↓
Validator
↓
Handler

```

Validation failures must prevent command execution.

---

# Transaction Boundary

Commands execute within a **single transaction scope**.

Rules:

- one command modifies one aggregate
- domain events are emitted after state change
- event publishing occurs after successful persistence

---

# Relationship With Vertical Slices

Each vertical slice implements:

```

Command / Query
Validator
Handler
Endpoint

```

Example:

```

features/calendar/schedule-event
ScheduleEventCommand
ScheduleEventValidator
ScheduleEventHandler
ScheduleEventEndpoint

```

---

# Architectural Role

The application model ensures that:

- system capabilities are explicit
- domain logic remains isolated
- modules interact through events
- vertical slices remain independent

It provides the **execution backbone of the DomusMind platform**.
