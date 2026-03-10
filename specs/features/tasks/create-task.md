# Create Task

Context: Tasks  
Aggregate: Task  
Capability: CreateTask

## Command

CreateTask

Fields:

- TaskId
- FamilyId
- Title
- AssigneeId (optional)
- DueDate (optional)
- ResponsibilityDomainId (optional)

Example

{
  "taskId": "ULID",
  "familyId": "ULID",
  "title": "Buy groceries",
  "assigneeId": "ULID",
  "dueDate": "2026-03-15T18:00:00Z"
}

## Rules

- TaskId must be unique
- Family must exist
- Assignee must belong to the family (if provided)
- Title must not be empty

## State Change

Creates a new Task aggregate.

Initial state:

- status = pending
- assigned member optional
- origin = manual

## Events

TaskCreated

## Success

Task exists and may be assigned, started, or completed.

## Failure

ValidationError  
FamilyNotFound  
MemberNotFound  
TaskAlreadyExists