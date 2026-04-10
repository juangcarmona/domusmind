# Surface Spec — Lists

## Purpose

Provide a fast, dense, reusable household list surface for grouped household memory.

This surface answers:

- what lists exist
- which list matters now
- what remains unchecked
- what should be added, checked, or updated next

Lists represent persistent grouped household memory.

They are:

- reusable
- toggle-based
- shared by default, optionally private
- unscheduled
- persistent across uses

They are not:

- a task manager
- a calendar
- a scheduling system
- a per-item reminder system
- an assignment or ownership model

---

## Depends On

- `docs/00_product/experience.md`
- `docs/00_product/surface-system.md`
- `docs/04_contexts/shared-lists.md`

---

## Entry Points

- main navigation → `Lists`
- contextual reference from Agenda when a plan has a related list
- deep link to a specific list

---

## Conceptual Model

### What a List Is

A list is a named household memory container for grouped items.

A list exists to answer: *"What should be remembered, bought, checked, or prepared next time?"*

Lists are:

- grouped by context or purpose, not by time
- independent objects — they can exist without any link to a plan or area
- optionally contextual — they may be associated with a plan or area
- usually untimed
- persistent across uses
- reusable by design

Common household examples:

- groceries
- packing for a trip
- preparation for an event
- restocking essentials
- recurring situation checklists

### What a List Is Not

A list is not a task board.

A list item is not a task by default.

A list does not become part of Agenda just because it is linked to a plan.

A list linked to a plan remains a list. Its items do not become scheduled tasks.

Lists must not acquire scheduling, assignment, prioritization, or reminder semantics.
These belong to Agenda and Tasks.

### Lifecycle

A list follows this lifecycle:

1. **created** — with a name, standalone or from context
2. **used and filled** — items are added and the list grows
3. **active** — items are checked and unchecked during use
4. **rested** — items remain, ready for next use
5. **archived** — if the list is no longer needed

Items are consumable.
Lists are often not — they are meant to be reused.

Reuse is more important than one-time completion.

### Household Boundaries

The four core axes of DomusMind are:

- **Agenda** → owns TIME
- **Tasks** → owns EXECUTION
- **Areas** → owns OWNERSHIP
- **Lists** → owns GROUPED MEMORY

No semantic collapse is permitted across these axes.

---

## Role

Lists is the household surface for reusable grouped memory.

It is optimized for:

- scan speed
- quick capture
- quick toggle state
- low-friction inspection
- low-friction switching between lists

The active list is the working surface.

---

## Core Principles

- content over chrome
- one row = one item
- quick add always visible
- detail is secondary
- counts are visible before opening a list
- completed items stay accessible but compressed
- theming supports recognition, not decoration
- density is high without feeling hostile
- capture must be faster than remembering
- grouping matters more than scheduling
- reuse is more important than one-time usage
- context enriches but does not define the list

---

## Surface Structure

Desktop default:

1. app navigation rail
2. list switcher pane
3. active list pane
4. optional inspector

Mobile default:

1. active list
2. list switcher via drawer/sheet
3. item detail via bottom sheet

This surface must use the shared product shell.

---

## Layout Zones

### 1. List Switcher

Shows available lists for the household.

Each row should show:

- list icon or color marker
- list name
- unchecked count
- optional area or linked-plan cue

The switcher must be compact and scannable.

### 2. Active List Header

Shows:

- list name
- unchecked count
- optional area chip
- optional linked plan chip
- search when needed
- compact overflow actions

The header must stay compact.
No oversized banner.

### 3. Active List Body

Shows the items of the selected list.

Default structure:

- unchecked items
- completed items collapsed behind a compact section

The body must prioritize fast scan and toggle.

### 4. Quick Add Bar

Always visible in the current working context.

Used to add a new item directly into the active list.

### 5. Optional Inspector

Used for secondary item detail without leaving the list.

Typical inspector content:

- item title
- note
- quantity
- optional links or metadata

---

## Item Model

An item in a list has a strict model.

Required:

- name

Optional:

- quantity
- note
- checked state

The item model explicitly does not include:

- due date
- reminder
- assignee
- priority
- recurrence per item
- attachments
- comments
- status systems

These fields do not belong to list items.
If they appear in implementation, they must be rejected.

Items must not be converted to tasks.

---

## Creation Model

Creating a list must be frictionless.

A list can be created with a name only.
No required metadata at creation time.

A list can be:

- created standalone from the Lists surface
- created from context (from an event detail, from an area)
- linked later to an existing plan or area

Linking is always optional and never required.

Items are added inline.
No modal is required for basic item capture.
Quick add must support sequential entry without interruption.

---

## Relationship with Agenda

A plan in Agenda may reference a related list.

When a plan has a related list:

- Agenda shows a compact reference cue — the list name and unchecked count
- selecting the cue navigates to the full list in the Lists surface
- Agenda does not expand list items inline as plan content
- Agenda does not convert list items to tasks
- Agenda does not display list items in the timeline

A list linked to a plan remains a list.
Its items remain list items.
No temporal or execution semantics are inherited from the link.

---

## Relationship with Areas

An area may have one or more associated lists.

When a list is associated with an area:

- the list appears as contextual memory for that area
- it represents what is remembered or tracked in the context of that area
- it is not a workload view
- it is not an execution view
- it does not affect ownership or responsibility assignment

An area-linked list is navigable from the Areas inspector but lives and behaves the same as any other list.

---

## List Switcher Behavior

The switcher is the index of household lists.

It should answer immediately:

- which lists exist
- which have remaining work
- which list is active

Rules:

- active list is visually clear
- unchecked count is always visible
- long list names truncate cleanly
- empty lists remain visible
- switching lists preserves shell context

Do not use card previews.

---

## Active List Behavior

The selected list fills the main working area.

The list should feel like a working surface, not a showcase page.

Rules:

- unchecked items appear first
- completed items are de-emphasized
- row density stays high
- scrolling is smooth and uninterrupted
- selection does not navigate away by default

---

## Row Model

Each item row represents one list item.

Default row content:

- toggle control
- item title
- optional secondary metadata
- optional quick affordance

Examples of secondary metadata:

- quantity
- short note
- small contextual hint

Rules:

- one row = one item
- row height stays compact
- second line only when useful
- metadata never dominates the title
- row tap opens detail
- toggle is immediate

Avoid card-per-item layout.

---

## Completed Items

Completed items must remain available but compressed.

Default pattern:

- collapse under `Completed (N)`

Rules:

- completed items do not dominate the main view
- expanding completed items stays lightweight
- completed items preserve order and state
- bulk visibility is possible without cluttering the default view

---

## Quick Add

Quick add is mandatory.

Rules:

- always visible in the active list context
- supports fast sequential entry
- requires minimal fields by default
- adding an item does not open a full modal
- focus returns cleanly for repeated capture

Quick add should optimize for speed over metadata completeness.

---

## Detail Interaction

Detail is secondary.

Desktop:

- selecting an item opens it in the inline inspector
- detail stays within the list context
- editing is in place — no modal required for name or simple fields

Mobile:

- selecting an item opens a bottom sheet
- bottom sheet stays compact
- close returns focus to the list

Detail panels must not expose fields that are not in the item model.
No due date field.
No assignee field.
No priority.

---

## Anti-Patterns

The following behaviors are explicitly rejected for the Lists surface.

**Semantic drift:**

- turning list items into tasks
- adding due dates or reminders to list items
- treating a list linked to a plan as a task batch for that plan
- displaying list items in the Agenda timeline

**Layout drift:**

- card-based layout for list items
- oversized rows with too much whitespace
- hero sections or decorative headers above the list

**Interaction drift:**

- requiring a modal to add a basic item
- opening a full separate page to view a list item
- requiring complex metadata before capture

**Model drift:**

- making lists person-centric instead of household-centric
- treating lists as disposable task batches
- forcing context linkage as a required step
- importing priority or assignment systems from Tasks
