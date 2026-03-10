# Create Routine

Context: Tasks  
Aggregate: Routine  
Capability: CreateRoutine

## Command

CreateRoutine

Fields:

- RoutineId
- FamilyId
- Title
- RecurrenceRule
- AssigneeId (optional)
- ResponsibilityDomainId (optional)

Example

{
  "routineId": "ULID",
  "familyId": "ULID",
  "title": "Weekly grocery shopping",
  "recurrenceRule": "weekly",
  "assigneeId": "ULID"
}

## Rules

- RoutineId must be unique
- Family must exist
- RecurrenceRule must be valid
- Assignee must belong to the family (if provided)

## State Change

Creates a new Routine aggregate.

Routine defines a recurring task template.

Future tasks may be generated according to the recurrence rule.

## Events

RoutineCreated

## Success

Routine exists and may generate tasks.

## Failure

ValidationError  
FamilyNotFound  
MemberNotFound  
RoutineAlreadyExists
