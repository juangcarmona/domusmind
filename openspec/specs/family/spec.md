<!-- pdac-scope: cited -->

# Family Specification

## Purpose

The Family context defines the household unit and its internal human structure.

A **Family** is the primary identity boundary of DomusMind. It is the root organizational unit. All other contexts depend on Family for participant identity but may not modify its structure.

A **Member** is a person belonging to a household. Members are independent participants who may be assigned tasks, hold responsibilities, and participate in plans. A member has a `MemberRole`: `Adult`, `Child`, `Caregiver`, or `Pet`. A member may additionally be designated as a **manager** — a boolean flag that grants authority to perform administrative actions (editing other members, provisioning access, etc.). How a member is first designated as manager is not specified in available source documents (see NOTE N4).

<!-- pdac-drift ids="TERM-MEMBER-ROLE, BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE" summary="spec lists a Caregiver role; the model has only Adult, Child and Pet (decided: Q-0021)" -->

<!-- pdac-drift ids="TERM-MANAGER, BR-FAMILY-CREATOR-IS-FIRST-MANAGER, BR-FAMILY-MANAGER-MUST-BE-ADULT" summary="spec leaves manager designation unspecified; the model makes the household creator its first manager and lets only adults be designated manager when added or edited (decided: Q-0022)" -->

A **Pet** is a non-human entity belonging to the household. In Phase 1, pets are represented as household members with `MemberRole = Pet`. The domain design describes Pet as a distinct entity (with its own identity separate from Member); Phase 1 uses `FamilyMember` with `MemberRole = Pet` instead. The `Pet` role carries specific operational constraints: pets cannot be provisioned with system access, cannot receive task assignments, cannot hold responsibility ownership, and are excluded from household coordination views (Agenda rows, member timelines) in V1. Pets may be added as event participants (e.g. a vet appointment).

A **Relationship** is a structural link between two family members. Relationships express household dependency and care structures (parent→child, spouse↔spouse, sibling↔sibling, caregiver→dependent). Relationship management is a V1.1 capability (see NOTE N5).

The Family context is the upstream identity provider. It emits more than it consumes. No downstream context may create, modify, or remove family structure.

---

## Requirements

### Requirement: Household Creation

A household SHALL be created with a name as the only required input.

On creation, the household starts with empty member, pet, and relationship sets. The household receives a stable, unique identifier that never changes.

<!-- pdac:cite id="FR-FAMILY-CREATE-HOUSEHOLD" digest="sha256:071bfc92159082ffe20b6f4f332dab2b475a9a962315a844b53c3b3b13a36817" -->

<!-- pdac:cite id="UC-FAMILY-CREATE-HOUSEHOLD" digest="sha256:851a62a8666aa15cfc022e27ba7f4829fb95d0118f2b20e818ce7cf5e8f593a5" -->

<!-- pdac:cite id="BR-FAMILY-HOUSEHOLD-NAME-REQUIRED" digest="sha256:facc4fa128293ee83eddc9dd383492d43892f6027551a91c08ecd1f0bccb1f93" -->

<!-- pdac:cite id="BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD" digest="sha256:ff68e8eb9bbbb83f788bb468a009dd2835b6721c9fa991fda8cfe9ed84930aee" -->

<!-- pdac:cite id="TERM-HOUSEHOLD" digest="sha256:f71aa38813b387bc9962f78a716ad33a144b8b754494e9dd560afab3e5520a98" -->

<!-- pdac:cite id="SB-FAMILY-HOUSEHOLD-CREATED" digest="sha256:6f2155301aa61ff8a00e01aa8f4c4127593f59ca1f89410dcc46294cb9add82d" -->

<!-- pdac:cite id="SB-FAMILY-HOUSEHOLD-EMPTY-NAME-REJECTED" digest="sha256:b3f7c3dc8db89694942f1672cf70e2994c8b0c929e715ad645bd75bcdcab48f0" -->

<!-- pdac-drift ids="FR-FAMILY-CREATE-HOUSEHOLD, BR-FAMILY-CREATOR-IS-FIRST-MANAGER, UC-FAMILY-CREATE-HOUSEHOLD" summary="spec says a new household starts with an empty member set; the model makes the person who created it its first manager (decided: Q-0022)" -->

#### Scenario: Household is created

- GIVEN a valid name is provided
- WHEN a household is created
- THEN a Family is established with a stable FamilyId
- AND the household has an empty member set, pet set, and relationship set
- AND a `FamilyCreated` event is emitted

#### Scenario: Household creation fails with an empty name

- WHEN a household creation is attempted with an empty or missing name
- THEN the household is not created
- AND a validation error is returned

---

### Requirement: Member Addition

A household SHALL be able to add a new member with a name and a role.

Valid roles are: `Adult`, `Child`, `Caregiver`, `Pet`. A member receives a unique identifier within the household. Once added, the member becomes part of the household roster and may be referenced by other contexts through their MemberId.

Optional inputs: birth date, notes.

<!-- pdac:cite id="FR-FAMILY-ADD-MEMBER" digest="sha256:5426d5a58aa9cecdb5834017e1e3502de59daf5d00b15b313a42847faaa604af" -->

<!-- pdac:cite id="UC-FAMILY-ADD-MEMBER" digest="sha256:2ebb408fcc96b56804358ac8104f1733cbcaeb37544608d3e4fb030cd178944b" -->

<!-- pdac:cite id="BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE" digest="sha256:2fb3454c04e677212f4a1984e55b7a10a8d37c8b5202c974fbb3b2dec24ba518" -->

<!-- pdac:cite id="BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD" digest="sha256:ff68e8eb9bbbb83f788bb468a009dd2835b6721c9fa991fda8cfe9ed84930aee" -->

<!-- pdac:cite id="TERM-MEMBER" digest="sha256:77bc7d6cc27b0d2a5593bad66f4d0dc8b66c13cd30b6757cacf9abccec937121" -->

<!-- pdac:cite id="TERM-MEMBER-ROLE" digest="sha256:e109ef46ac8bb6549b51c0017367fca4df999d8b66d08373bf1cca82d9c2c475" -->

<!-- pdac:cite id="SB-FAMILY-MEMBER-ADDED" digest="sha256:464c27f949f9bc0e59baddeeabd8ecb2b38bff185a65f9fef799119153bbc4cf" -->

<!-- pdac:cite id="SB-FAMILY-DUPLICATE-MEMBER-REJECTED" digest="sha256:36287efaae249207b0ab31f3a6b2434f3c89249db4cd6f33eda95c652684352e" -->

<!-- pdac:cite id="SB-FAMILY-INVALID-ROLE-REJECTED" digest="sha256:5d95412afc930d72158a3d7fe280b787892b5dbf8e678008b38160b2ff454892" -->

<!-- pdac:cite id="SB-FAMILY-MEMBER-UNKNOWN-HOUSEHOLD-REJECTED" digest="sha256:560d8bbd212e4eedaafeaed93719a03bc66340b2f347657bb535372063489b7f" -->

<!-- pdac-drift ids="TERM-MEMBER-ROLE, BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE, FR-FAMILY-ADD-MEMBER" summary="spec lists a Caregiver role; the model has only Adult, Child and Pet (decided: Q-0021)" -->

<!-- pdac-drift ids="FR-FAMILY-ADD-MEMBER, BR-FAMILY-MANAGERS-ADMINISTER, BR-FAMILY-MANAGER-MUST-BE-ADULT" summary="spec lets the household add a member without restriction; the model lets only a manager add people (decided: Q-0024) and lets an adult be designated manager on add (decided: Q-0022)" -->

#### Scenario: A member is added to the household

- GIVEN a household exists
- WHEN a member is added with a valid name and role
- THEN the member is added to the household with a unique MemberId
- AND a `MemberAdded` event is emitted

#### Scenario: Member addition fails with a duplicate MemberId

- GIVEN a household exists with an existing member
- WHEN a new member is submitted with the same MemberId
- THEN the member is not added
- AND a validation error is returned

#### Scenario: Member addition fails with an invalid role

- WHEN a member is submitted with a role not in the allowed set
- THEN the member is not added
- AND a validation error is returned

#### Scenario: Member addition fails if the household does not exist

- WHEN a member is submitted for a non-existent FamilyId
- THEN the operation is rejected

---

### Requirement: Member Removal _(V1.1)_

> **Scope note:** This capability is explicitly deferred to V1.1 in `specs/system/system-spec.md`. Member removal requires validating open task assignments and participant references. It is modeled in the domain but not exposed via API in V1.

A household SHALL be able to remove an existing member.

A removed member cannot participate in new relationships. Removing a member does not cascade into other contexts — those contexts reference by ID and must handle identity resolution independently.

<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:661a8bfff912e490d64710e11b333119cb4143fdf597ca2672523e0bd7fae034" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:16a5dbe3d9df35349d7f302b5f2399945e2395332587d8883fc71c4406754cf7" -->

<!-- pdac:cite id="BC-FAMILY" digest="sha256:3bf7d1e482866302ac3725ac77eed6a5f37cebf2ba60a99323a37194c8f187ae" -->

#### Scenario: A member is removed from the household

- GIVEN a household exists with at least one member
- WHEN that member is removed
- THEN the member is no longer part of the household roster
- AND a `MemberRemoved` event is emitted

---

### Requirement: Member Core Details Update

A household manager SHALL be able to update a member's full name, role, and optional birth date.

Birth date, when provided, must be in the past. Only managers may perform this action. A member may edit their own profile details through a separate profile update path.

<!-- pdac:cite id="FR-FAMILY-UPDATE-MEMBER-DETAILS" digest="sha256:60ffad3045fb6e837afd501f62a73bf93feadcb2a5b253bfa378f4ff2b74f6af" -->

<!-- pdac:cite id="UC-FAMILY-UPDATE-MEMBER-DETAILS" digest="sha256:02ea0e682c88a9b50ec75e9f82aaf888ae5bcf959853e2e691375f4593706a23" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:16a5dbe3d9df35349d7f302b5f2399945e2395332587d8883fc71c4406754cf7" -->

<!-- pdac:cite id="BR-FAMILY-BIRTH-DATE-IN-PAST" digest="sha256:e0115d380b63512e91e99a3a38a7ced5ee1bf2bb170ef5a073ca4b7a584b07b8" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MANAGER" digest="sha256:12dacff31d93a8b994bddadabce84831a7ba4c8bf06eb8a15710b673cd66349b" -->

<!-- pdac:cite id="TERM-MANAGER" digest="sha256:c5bfc7b04717b03d178606f77a0ebe0c29c34ee76af10d4d8d459693d600277a" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-UPDATES-MEMBER" digest="sha256:6656d7d70669825cb2950c5f8d18d95d0ba25f1e4361df58b29cfd3f2c120e53" -->

<!-- pdac:cite id="SB-FAMILY-NON-MANAGER-UPDATE-REJECTED" digest="sha256:e4fa8ba6ba6b802bc5a914c5b46b7e59b5496e6b2e34cbba8bdb7ad1471ea3cb" -->

<!-- pdac:cite id="SB-FAMILY-FUTURE-BIRTH-DATE-REJECTED" digest="sha256:4c9c89bc305863769d6b96dfcd10e19dfc7a879076382857af9399bd038178b8" -->

<!-- pdac-drift ids="FR-FAMILY-UPDATE-MEMBER-DETAILS, BR-FAMILY-MANAGERS-ADMINISTER, SB-FAMILY-MEMBER-UPDATES-OWN-DETAILS, SB-FAMILY-NON-MANAGER-OWN-ROLE-REJECTED" summary="spec reserves core-detail updates to managers; the model lets a non-manager change their own name and birth date but not their own role (decided: Q-0023)" -->

#### Scenario: Manager updates a member's name and role

- GIVEN a household exists with a member
- AND the requesting user is a manager
- WHEN the member's name, role, or birth date is updated with valid values
- THEN the member record reflects the new values

#### Scenario: Non-manager cannot update another member's core details

- GIVEN a household exists with a member
- AND the requesting user is not a manager and is not the target member
- WHEN a core detail update is attempted
- THEN the update is rejected

#### Scenario: Birth date in the future is rejected

- WHEN a member update is submitted with a future birth date
- THEN the update is rejected
- AND a validation error is returned

---

### Requirement: Member Profile Update

A member SHALL be able to update their own optional contact details: preferred name, phone, email, and household note.

Managers may also update any member's profile details.

<!-- pdac:cite id="FR-FAMILY-UPDATE-MEMBER-PROFILE" digest="sha256:02beed02db0914578e148c0eb2bf0bbf17915facd8ce2173e0f1f2329e6b8cdf" -->

<!-- pdac:cite id="UC-FAMILY-UPDATE-MEMBER-DETAILS" digest="sha256:02ea0e682c88a9b50ec75e9f82aaf888ae5bcf959853e2e691375f4593706a23" -->

<!-- pdac:cite id="TERM-MEMBER-PROFILE" digest="sha256:2d384c7f86fe4dfaca088537c1ec827ef85b38268c48716dec1e1fe50c15d18e" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MEMBER" digest="sha256:18173726b69715b42a2c09ddd0c09876f486d1abefd93c3131e2cb0d6c4df860" -->

<!-- pdac:cite id="SB-FAMILY-MEMBER-UPDATES-OWN-PROFILE" digest="sha256:c4a52f54a9c1bd9a88ca976f5c469536413f65c664d4f95100248d37d77c26b6" -->

#### Scenario: Member updates their own profile

- GIVEN a household member is authenticated
- WHEN that member updates their own preferred name, phone, email, or household note
- THEN the profile is updated

---

### Requirement: Member Access Provisioning

A household manager SHALL be able to provision, disable, enable, and reset system access for members.

Access provisioning links a member's household identity to an authentication account. Pets cannot be provisioned with access.

The access state of a member is one of: `NoAccess`, `InvitedOrProvisioned`, `PasswordResetRequired`, `Active`, `Disabled`.

| State | Meaning |
|---|---|
| `NoAccess` | No authentication account linked |
| `InvitedOrProvisioned` | Account exists, password change required, never logged in |
| `PasswordResetRequired` | Account exists, password change required, has logged in before |
| `Active` | Account exists, not disabled, no forced password change |
| `Disabled` | Account exists but is disabled |

Rules:
- Only a manager may provision, disable, enable, or regenerate access for other members
- A manager may not disable their own access
- Pets cannot be provisioned with access

<!-- pdac:cite id="FR-FAMILY-MANAGE-MEMBER-ACCESS" digest="sha256:36fd2c62c621cc1599a9d60e84a7fbd4c40e424552eb70b12d03b6cb760ac4e8" -->

<!-- pdac:cite id="UC-FAMILY-PROVISION-MEMBER-ACCESS" digest="sha256:38ab99dfc5bc409197237451317d77eca49848183f2185f360a3d8d1f14622b7" -->

<!-- pdac:cite id="UC-FAMILY-DISABLE-MEMBER-ACCESS" digest="sha256:b31cfb989c835b046bd758ea13d41df4587b96aa9fe42c03579ed69f26ceea3a" -->

<!-- pdac:cite id="UC-FAMILY-ENABLE-MEMBER-ACCESS" digest="sha256:24e8b64d134a5cdeb33ff6e36896879c64a13f9f3d62f9371cc5cde4671e2a72" -->

<!-- pdac:cite id="UC-FAMILY-RESET-MEMBER-ACCESS" digest="sha256:51e1bcc957caa70b8af9863e0e7847087fe1d36e6d205b7025cad4ee151fc56a" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:16a5dbe3d9df35349d7f302b5f2399945e2395332587d8883fc71c4406754cf7" -->

<!-- pdac:cite id="BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS" digest="sha256:81f0e5a3d3df0bf834b5544c2229c0ccdbe33d76e7d7bbfd2e37f3cb739029b7" -->

<!-- pdac:cite id="BR-FAMILY-PET-RESTRICTIONS" digest="sha256:bb6786787a592043220d16abdba4e60d0260c2c8597351c67e41cb24fe53d9ba" -->

<!-- pdac:cite id="TERM-MEMBER-ACCESS" digest="sha256:b7985c2011d508e2f15b31f8079b9dd54cd5c3603870e83324c4892601f1593b" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MANAGER" digest="sha256:12dacff31d93a8b994bddadabce84831a7ba4c8bf06eb8a15710b673cd66349b" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-PROVISIONS-ACCESS" digest="sha256:44a9e2ba5fdac89b35f94cf3d24127190aa1e3c70e6d7a0fb9e619a18edc0217" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-DISABLES-ACCESS" digest="sha256:6597572884c6b7cd0fbcc3078e42a4fb7104b7c885a71731a8bcc6929b1ab730" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-ENABLES-ACCESS" digest="sha256:c0fa5208d278db6694fe9bdd9cdc0702631842fe2a23b794d0aec7801557c6b0" -->

<!-- pdac:cite id="SB-FAMILY-PET-ACCESS-REJECTED" digest="sha256:f1fed230ba7bc8ed33bcd49f58bc64e6d6d5d3d70440a7cff1f39494d06233c0" -->

#### Scenario: Manager provisions access for a member

- GIVEN a member exists with `NoAccess` state
- AND the requesting user is a manager
- AND the member role is not Pet
- WHEN access is provisioned
- THEN the member is linked to an authentication account
- AND the member state becomes `InvitedOrProvisioned`

#### Scenario: Manager disables a member's access

- GIVEN a member exists with `Active` state
- AND the requesting user is a manager
- AND the target is not the requesting manager themselves
- WHEN access is disabled
- THEN the member state becomes `Disabled`

#### Scenario: Manager enables a member's access

- GIVEN a member exists with `Disabled` state
- AND the requesting user is a manager
- WHEN access is enabled
- THEN the member state becomes `Active`

#### Scenario: Provisioning a Pet is rejected

- GIVEN a member with role `Pet`
- WHEN access provisioning is attempted
- THEN the operation is rejected

---

### Requirement: Pet Registration

A household SHALL be able to register a pet by adding a member with `MemberRole = Pet`.

In Phase 1, pets are stored as `FamilyMember` entries with the `Pet` role — not as a distinct Pet entity. The domain design describes Pet as a separate entity; Phase 1 collapses this into the member model (see NOTE N6).

Pets in V1:
- cannot be provisioned with system access
- cannot receive task assignments
- cannot hold responsibility ownership
- are excluded from Agenda household rows and member coordination timelines
- may be added as event participants (e.g. vet appointments)
- appear in the member directory as a distinct group (after Adults, Caregivers, and Children)

<!-- pdac:cite id="FR-FAMILY-REGISTER-PET" digest="sha256:0d762688d45680d0048ed5c38f880774d6fadc4e22dee2b12a514802cb470c77" -->

<!-- pdac:cite id="UC-FAMILY-REGISTER-PET" digest="sha256:5382e2200582435cdb113ff8baad906050250e6deee7c67f7c0a5c64c00cd4dc" -->

<!-- pdac:cite id="BR-FAMILY-PET-RESTRICTIONS" digest="sha256:bb6786787a592043220d16abdba4e60d0260c2c8597351c67e41cb24fe53d9ba" -->

<!-- pdac:cite id="TERM-PET" digest="sha256:5ac955dda87a76f7851192ffdb474d3baf7b1a82757c17f095017b3d4f5ce55b" -->

<!-- pdac:cite id="BR-FAMILY-DIRECTORY-ORDER" digest="sha256:750b94ea238912c06a816c58f3638afa817f69ae6457b7810f3c4e2ac1c018b3" -->

<!-- pdac:cite id="SB-FAMILY-PET-ADDED" digest="sha256:8eebb83891510e8758e183c8ee45aea5c2a1e5be4a673d88bcc52ded797a396e" -->

<!-- pdac:cite id="SB-FAMILY-PET-ACCESS-REJECTED" digest="sha256:f1fed230ba7bc8ed33bcd49f58bc64e6d6d5d3d70440a7cff1f39494d06233c0" -->

<!-- pdac-drift ids="FR-FAMILY-REGISTER-PET, BR-FAMILY-DIRECTORY-ORDER, TERM-MEMBER-ROLE" summary="spec lists pets after Adults, Caregivers and Children; the model has no Caregiver role, so pets follow adults and children (decided: Q-0021)" -->

<!-- pdac-drift ids="FR-FAMILY-REGISTER-PET, BR-FAMILY-MANAGERS-ADMINISTER, UC-FAMILY-REGISTER-PET" summary="spec lets the household register a pet without restriction; the model lets only a manager register pets (decided: Q-0024)" -->

#### Scenario: A pet is added to the household

- GIVEN a household exists
- WHEN a member is added with a valid name and `MemberRole = Pet`
- THEN the pet is part of the household with a unique MemberId
- AND a `MemberAdded` event is emitted

#### Scenario: A pet is removed from the household

- GIVEN a household exists with a registered pet
- WHEN the pet member is removed
- THEN the pet is no longer part of the household
- AND a `MemberRemoved` event is emitted

---

### Requirement: Relationship Assignment _(V1.1)_

> **Scope note:** This capability is explicitly deferred to V1.1 in `specs/system/system-spec.md`. Relationship semantics are modeled in the domain but not exposed via API or UI in V1.

A household SHALL be able to define a structural relationship between two of its members.

Both parties must be existing members of the same household. Duplicate relationships of the same type between the same entities are not allowed. Valid relationship types include: parent→child, spouse↔spouse, sibling↔sibling, caregiver→dependent.

<!-- pdac:cite id="BC-FAMILY" digest="sha256:3bf7d1e482866302ac3725ac77eed6a5f37cebf2ba60a99323a37194c8f187ae" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:16a5dbe3d9df35349d7f302b5f2399945e2395332587d8883fc71c4406754cf7" -->

<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:661a8bfff912e490d64710e11b333119cb4143fdf597ca2672523e0bd7fae034" -->

#### Scenario: A relationship is assigned between two members

- GIVEN a household exists with two distinct members
- WHEN a relationship of a valid type is assigned between them
- THEN the relationship is recorded in the household structure
- AND a `RelationshipAssigned` event is emitted

#### Scenario: Relationship assignment fails if either member does not exist

- WHEN a relationship references a MemberId not in the household
- THEN the assignment is rejected

#### Scenario: Duplicate relationship is rejected

- GIVEN a relationship of a given type already exists between two members
- WHEN the same relationship type is submitted for the same pair
- THEN the assignment is rejected

#### Scenario: Relationship assignment fails if both parties are the same member

- WHEN a relationship is submitted with the same member ID for both parties
- THEN the assignment is rejected

#### Scenario: A relationship is removed

- GIVEN a relationship exists between two members
- WHEN the relationship is removed
- THEN it is no longer part of the household structure
- AND a `RelationshipRemoved` event is emitted

---

### Requirement: Household Member Directory

A household member SHALL be able to view the member directory of their household.

The directory is ordered: Adults and Caregivers first, then Children, then Pets. Within each group, managers appear before other members; within that ordering, entries are alphabetical by name.

The directory includes server-computed access flags. Clients must not re-derive permission or access state from raw fields.

<!-- pdac:cite id="FR-FAMILY-MEMBER-DIRECTORY" digest="sha256:5b37af39b18fe5310ef69335a23e89cc24b2e79ebe0e1cf76ec60e77138cb52e" -->

<!-- pdac:cite id="UC-FAMILY-VIEW-MEMBER-DIRECTORY" digest="sha256:aae017ca2a22732c1705d869394aec7bd7b5112addde5aafcc689622cb005f40" -->

<!-- pdac:cite id="BR-FAMILY-DIRECTORY-ORDER" digest="sha256:750b94ea238912c06a816c58f3638afa817f69ae6457b7810f3c4e2ac1c018b3" -->

<!-- pdac:cite id="TERM-MEMBER-DIRECTORY" digest="sha256:585b936b6a37773a3aba6af87bd15a1a1d0650672fad3e850a3f16b9d6ca2bc0" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MEMBER" digest="sha256:18173726b69715b42a2c09ddd0c09876f486d1abefd93c3131e2cb0d6c4df860" -->

<!-- pdac-drift ids="BR-FAMILY-DIRECTORY-ORDER, FR-FAMILY-MEMBER-DIRECTORY, TERM-MEMBER-ROLE" summary="spec groups Adults and Caregivers first; the model has no Caregiver role and orders adults, then children, then pets (decided: Q-0021)" -->

---

### Requirement: Identity Boundary Enforcement

The Family context SHALL be the sole owner of household structure.

No other context may create, modify, or remove members, pets, or relationships. Other contexts reference family entities by ID only. Cross-context reactions happen through domain events.

<!-- pdac:cite id="FR-FAMILY-IDENTITY-BOUNDARY" digest="sha256:f33de9aaebe308d06ca7cef4af60ceaa5fac65e1109bb40add0ab767a32c47a3" -->

<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:661a8bfff912e490d64710e11b333119cb4143fdf597ca2672523e0bd7fae034" -->

<!-- pdac:cite id="BC-FAMILY" digest="sha256:3bf7d1e482866302ac3725ac77eed6a5f37cebf2ba60a99323a37194c8f187ae" -->

---

## Domain Events

| Event | Emitted when |
|---|---|
| `FamilyCreated` | A new household is created |
| `MemberAdded` | A member is added to the household |
| `MemberRemoved` | A member is removed from the household |
| `MemberAdded` (role=Pet) | A pet is registered in Phase 1; the domain design describes a distinct `PetAdded` event — see NOTE N6 |
| `MemberRemoved` (role=Pet) | A pet is removed in Phase 1; domain design describes a distinct `PetRemoved` event — see NOTE N6 |
| `RelationshipAssigned` | A relationship is recorded (V1.1) |
| `RelationshipRemoved` | A relationship is removed (V1.1) |

---

## Invariants

- A Family must have a stable, unique FamilyId
- A member belongs to exactly one family
- Member IDs must be unique within the family
- In Phase 1, pets are members with `MemberRole = Pet`; pet identity uses MemberId and is covered by the member ID uniqueness invariant
- Relationships must reference existing members of the same family
- A relationship cannot reference the same member on both sides
- Duplicate relationships of the same type between the same entities are not allowed
- A removed member cannot participate in new relationships

---

## Notes

**N1 — Dependent entity status**: `docs/04_contexts/family.md` describes `Dependent` as a separate internal entity. However, `specs/system/system-spec.md` lists only `create-family` and `add-member` in the V1 Family feature set — there is no `add-dependent` in V1. `specs/features/family/member-management.md` confirms Phase 1 uses `FamilyMember` as the single member entity with `Child` and `Caregiver` as roles; the `Dependent` entity was designed but never shipped as a V1 capability. The associated commands (`AddDependent`, `RemoveDependent`) and events (`DependentAdded`, `DependentRemoved`) belong to the domain design and a future roadmap phase. This spec reflects the Phase 1 implementation state: children and care recipients are `Member` entities with `MemberRole = Child` or `Caregiver`.

<!-- pdac-drift ids="TERM-MEMBER-ROLE, TERM-MEMBER, BC-FAMILY" summary="spec treats Caregiver as a Phase 1 member role; the model has no Caregiver role (decided: Q-0021)" -->

**N2 — `UpdateFamilySettings` command**: Listed in `docs/04_contexts/family.md` but not detailed in any spec or feature document. Behavior is undefined. Excluded from this spec.

<!-- pdac-drift ids="FR-FAMILY-HOUSEHOLD-SETTINGS, UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS, TERM-HOUSEHOLD-SETTINGS" summary="spec excludes household settings as undefined; the model defines name, primary language, first day of week and date format, changed only by managers (decided: Q-0025)" -->

**N3 — Manager self-removal and last-manager constraint**: No source document specifies whether a manager can remove themselves, whether a household must always have at least one manager, or what happens when the last manager is removed. Excluded pending clarification.

**N4 — Manager designation**: The `IsManager` boolean flag determines which members may perform administrative actions (edit other members, provision/disable access, etc.). Available source documents do not specify how a member is first designated as manager at household creation, or whether manager status can be granted or revoked through a distinct capability. Excluded pending clarification.

<!-- pdac-drift ids="TERM-MANAGER, BR-FAMILY-CREATOR-IS-FIRST-MANAGER, BR-FAMILY-MANAGER-MUST-BE-ADULT" summary="spec leaves first-manager designation open; the model makes the creator the first manager and sets the designation when adding or editing an adult (decided: Q-0022)" -->

**N5 — Relationship assignment deferral**: `specs/system/system-spec.md` explicitly defers `assign-relationship` and `remove-member` to V1.1, citing cascading complexity. The relationship model (Relationship entity, RelationshipType, RelationshipAssigned/Removed events) is fully modeled in the domain but not exposed via V1 API or UI.

**N6 — Pet entity vs. Pet role**: `docs/04_contexts/family.md` describes Pet as a separate aggregate with its own `PetId`, `PetAdded`, and `PetRemoved` events. `specs/features/family/member-management.md` Phase 1 baseline documents `MemberRole` with values `Adult`, `Child`, `Caregiver`, `Pet` — pets are stored as `FamilyMember` and their identity uses `MemberId`. Phase 1 emits `MemberAdded`/`MemberRemoved` for pet operations. The separate Pet entity model and its associated events (`PetAdded`, `PetRemoved`) represent a domain design goal deferred beyond Phase 1.

**N7 — Auth identity vs. domain identity**: The authentication identity of a user and the household `MemberId` are deliberately separate concerns. Auth identity is linked to a member via `AuthUserId` but the Family context does not own authentication. This spec describes the household identity side only.

**N8 — Onboarding flow**: How a household is created during initial user onboarding (e.g. whether it is triggered automatically or by explicit user action) is not described in the primary source documents. Excluded pending clarification.

---

## Sources

- `docs/04_contexts/family.md`
- `docs/03_domain/ubiquitous-language.md`
- `docs/03_domain/context-map.md`
- `specs/features/family/create-family.md`
- `specs/features/family/add-member.md`
- `specs/features/family/assign-relationship.md`
- `specs/features/family/member-management.md`
- `00_product/surfaces/settings.md`
