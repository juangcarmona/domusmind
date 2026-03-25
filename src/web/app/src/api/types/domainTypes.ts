export interface HouseholdAreaItem {
  areaId: string;
  name: string;
  color: string;
  primaryOwnerId: string | null;
  primaryOwnerName: string | null;
  secondaryOwnerIds: string[];
}

export interface HouseholdAreasResponse {
  areas: HouseholdAreaItem[];
}

export interface CreateResponsibilityDomainRequest {
  name: string;
  familyId: string;
}

export interface CreateResponsibilityDomainResponse {
  responsibilityDomainId: string;
  familyId: string;
  name: string;
  color: string;
  createdAtUtc: string;
}

export interface AssignPrimaryOwnerRequest {
  memberId: string;
}

export interface AssignSecondaryOwnerRequest {
  memberId: string;
}

export interface RemoveSecondaryOwnerResponse {
  responsibilityDomainId: string;
  memberId: string;
}

export interface TransferResponsibilityRequest {
  newPrimaryOwnerId: string;
}

export interface RenameResponsibilityDomainRequest {
  name: string;
}

export interface RenameResponsibilityDomainResponse {
  responsibilityDomainId: string;
  name: string;
}

export interface UpdateResponsibilityDomainColorRequest {
  color: string;
}

export interface UpdateResponsibilityDomainColorResponse {
  responsibilityDomainId: string;
  color: string;
}

export interface CreateTaskRequest {
  title: string;
  familyId: string;
  description?: string;
  dueDate?: string | null;
  dueTime?: string | null;
  color?: string;
  areaId?: string | null;
}

export interface CreateTaskResponse {
  taskId: string;
  familyId: string;
  title: string;
  description: string | null;
  dueDate: string | null;
  status: string;
  color: string;
  areaId: string | null;
  createdAtUtc: string;
}

export interface AssignTaskRequest {
  assigneeId: string;
}
