export interface MemberFormValues {
  name: string;
  role: string;
  birthDate: string;
  isManager: boolean;
}

export interface ProfileFormValues {
  preferredName: string;
  primaryPhone: string;
  primaryEmail: string;
  householdNote: string;
  avatarIconId: number | null;
  avatarColorId: number | null;
}

/** Combined form values for the unified person edit modal. */
export interface UnifiedPersonFormValues {
  // core identity
  name: string;
  role: string;
  birthDate: string;
  isManager: boolean;
  // profile / avatar
  preferredName: string;
  primaryPhone: string;
  primaryEmail: string;
  householdNote: string;
  avatarIconId: number | null;
  avatarColorId: number | null;
}

export const MEMBER_ROLES = ["Adult", "Child", "Pet"] as const;
export const ADD_MEMBER_ROLES = MEMBER_ROLES;
