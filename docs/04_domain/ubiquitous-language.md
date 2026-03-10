# DomusMind — Ubiquitous Language

## Purpose

This document defines the **shared vocabulary of the DomusMind domain**.

All code, documentation, APIs, and discussions about the system must use these terms consistently.

The goal is to maintain **conceptual clarity across the system**.

---

# Core Concepts

## Family

A **Family** is the primary organizational unit of the system.

A family represents a group of people managing a shared household and its associated responsibilities, resources, and logistics.

A family acts as the root boundary for most system operations.

---

## Member

A **Member** is a person belonging to a family.

Members may represent:

- adults
- children
- dependents
- caregivers

Members participate in responsibilities, events, tasks, and routines.

---

## Dependent

A **Dependent** is an entity requiring care or responsibility from family members.

Examples:

- children
- elderly relatives
- pets

Dependents generate tasks, events, and responsibilities.

---

## Responsibility Domain

A **Responsibility Domain** represents an area of household management.

Examples:

```

food
school
finances
maintenance
travel
pets
administration

```

Responsibility domains distribute cognitive ownership across the family.

---

## Responsibility Assignment

A **Responsibility Assignment** defines which members manage a responsibility domain.

Assignments may include:

- primary owner
- secondary owners
- participants

---

## Event

An **Event** represents something that occurs at a specific point in time.

Examples:

```

school excursion
medical appointment
meeting
exam
trip
maintenance visit

```

Events appear on the household timeline.

---

## Timeline

The **Timeline** represents the chronological sequence of events affecting the household.

It aggregates events from multiple contexts into a unified view.

---

## Task

A **Task** is an action that must be completed.

Tasks may originate from:

- events
- routines
- responsibilities
- manual input

Tasks may be assigned to members.

---

## Routine

A **Routine** represents a recurring operational pattern.

Examples:

```

weekly grocery shopping
school preparation
house cleaning
vehicle maintenance

```

Routines automatically generate tasks.

---

## Property

A **Property** represents a real estate asset managed by the family.

Examples:

```

primary residence
secondary home
rental property

```

Properties may generate expenses, maintenance events, and administrative obligations.

---

## Asset

An **Asset** represents a durable resource owned or managed by the household.

Examples:

```

vehicles
appliances
tools
equipment

```

Assets may require maintenance and generate events.

---

## Document

A **Document** represents an important record that must be stored or tracked.

Examples:

```

identity documents
insurance policies
contracts
certificates

```

Documents may include expiration or renewal dates.

---

## Contract

A **Contract** represents a formal agreement associated with the household.

Examples:

```

insurance policies
service subscriptions
property agreements

```

Contracts may generate renewals and financial obligations.

---

## Inventory

**Inventory** represents consumable resources used by the household.

Examples:

```

food
cleaning products
supplies

```

Inventory levels may trigger replenishment or shopping lists.

---

## Recipe

A **Recipe** represents a structured description of how to prepare a meal.

Recipes define ingredients and preparation steps.

---

## Meal Plan

A **Meal Plan** represents a scheduled set of meals across a time period.

Meal plans may generate shopping lists and cooking tasks.

---

## Pet

A **Pet** is a dependent animal belonging to the household.

Pets generate responsibilities such as:

- feeding
- veterinary care
- medication
- exercise

Pets participate in the household timeline.

---

## Reminder

A **Reminder** is a time-triggered notification.

Reminders typically originate from:

- events
- documents
- contracts
- maintenance schedules

---

## Domain Event

A **Domain Event** represents a meaningful state change within the system.

Examples:

```

FamilyCreated
MemberAdded
EventScheduled
ResponsibilityAssigned
TaskCompleted

```

Domain events allow different parts of the system to react without tight coupling.

---

# Architectural Concepts

These concepts appear in system design and code.

---

## Aggregate

An **Aggregate** is a cluster of domain objects treated as a single consistency boundary.

Aggregates enforce domain invariants and emit domain events.

Examples:

```

Family
Event
Property
MealPlan

```

---

## Aggregate Root

The **Aggregate Root** is the entry point to an aggregate.

All modifications must occur through the root.

Examples:

```

Family
Event
Property

```

---

## Bounded Context

A **Bounded Context** defines a boundary where a specific domain model applies.

Examples of DomusMind contexts:

```

Family
Responsibilities
Calendar
Tasks
Food
Properties
Administration
Pets
Finance

```

Each context owns its internal model and rules.

---

# Language Consistency Rules

To maintain clarity:

- Domain terms must be used consistently in code and documentation.
- API endpoints should reflect domain language.
- Database models must map clearly to domain concepts.
- Synonyms for domain entities should be avoided.

The ubiquitous language ensures that the **domain model remains coherent as the system evolves**.