<!-- pdac-scope: cited -->

# Family Specification

## Purpose

The Family context defines the household unit and its internal human structure.

A **Family** is the primary identity boundary of DomusMind. It is the root organizational unit. All other contexts depend on Family for participant identity but may not modify its structure.

A **Member** is a person belonging to a household. Members are independent participants who may be assigned tasks, hold responsibilities, and participate in plans. A member has one `MemberRole`: `Adult`, `Child`, or `Pet`. An adult member may additionally be designated as a **manager** — a designation that grants authority to administer the household. The person who creates a household is its first manager.

A **Pet** is a non-human entity belonging to the household. In Phase 1, pets are represented as household members with `MemberRole = Pet`. The domain design describes Pet as a distinct entity (with its own identity separate from Member); Phase 1 uses `FamilyMember` with `MemberRole = Pet` instead. The `Pet` role carries specific operational constraints: pets cannot be provisioned with system access, cannot receive task assignments, cannot hold responsibility ownership, and are excluded from household coordination views (Agenda rows, member timelines) in V1. Pets may be added as event participants (e.g. a vet appointment).

A **Relationship** is a structural link between two household members. Relationship management is a V1.1 capability (see NOTE N5).

The Family context is the upstream identity provider. It emits more than it consumes. No downstream context may create, modify, or remove family structure.

---

## Requirements

### Requirement: Household Creation

A household SHALL be created with a name as the only required input.

On creation, the household receives a stable, unique identifier that never changes. The person who creates it is added as its first manager. The household starts with no pets or relationships.

<!-- pdac:cite id="FR-FAMILY-CREATE-HOUSEHOLD" digest="sha256:8259a48f65b5d686938dfafadf21a74b76f15ce51047e2881fc5ec38904e1550" -->

<!-- pdac:cite id="UC-FAMILY-CREATE-HOUSEHOLD" digest="sha256:d79fa3c1d0ad710640ec508b6ea112a666ba571d4d4aa4c803811eb3e260c84f" -->

<!-- pdac:cite id="BR-FAMILY-HOUSEHOLD-NAME-REQUIRED" digest="sha256:3504ade28734b53d8d9d302bc629955c1501531e1dfa0623647020b63ff1e435" -->

<!-- pdac:cite id="BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD" digest="sha256:3f0596b6442473217955638b2a9fb8792fcaedb11444ae99d1bb7fde6dc27ec4" -->

<!-- pdac:cite id="TERM-HOUSEHOLD" digest="sha256:87493c2e4c72087ed204e99a656f157b648b7f2b5ad636e825bb6edd90716f76" -->

<!-- pdac:cite id="SB-FAMILY-HOUSEHOLD-CREATED" digest="sha256:1cf2fc581b8c12c785168444da9715949845d2fa1a079ddc54b21a8827566c64" -->

<!-- pdac:cite id="SB-FAMILY-HOUSEHOLD-EMPTY-NAME-REJECTED" digest="sha256:bf57d00fcf2852ff88ba981166339cbb2114e7bd2cf93d3a68f156db689f04b2" -->

#### Scenario: Household is created

- GIVEN a valid name is provided
- WHEN a household is created
- THEN a Family is established with a stable FamilyId
- AND the person who created it is added as its first manager
- AND the household has no pets or relationships
- AND a `FamilyCreated` event is emitted

#### Scenario: Household creation fails with an empty name

- WHEN a household creation is attempted with an empty or missing name
- THEN the household is not created
- AND a validation error is returned

---

### Requirement: Member Addition

A household manager SHALL be able to add a new member with a name and a role.

Valid roles are: `Adult`, `Child`, `Pet`. Only an adult may be designated as a manager. A member receives a unique identifier within the household. Once added, the member becomes part of the household roster and may be referenced by other contexts through their MemberId.

Optional inputs: birth date, notes.

<!-- pdac:cite id="FR-FAMILY-ADD-MEMBER" digest="sha256:1294e656772c13fe0e8d8b8bb56d09efe6ffcd20cdd352e542b6ad6c96a00059" -->

<!-- pdac:cite id="UC-FAMILY-ADD-MEMBER" digest="sha256:d352cbec06451a5b905c06fc15363baa2a770d93f112a301dea00929135d1f8f" -->

<!-- pdac:cite id="BR-FAMILY-PERSON-NEEDS-NAME-AND-ROLE" digest="sha256:656ea11d4202921e61a0e7475f50b82df70d9960eb3f767c47b26f6300fb550a" -->

<!-- pdac:cite id="BR-FAMILY-PERSON-UNIQUE-IN-HOUSEHOLD" digest="sha256:3f0596b6442473217955638b2a9fb8792fcaedb11444ae99d1bb7fde6dc27ec4" -->

<!-- pdac:cite id="TERM-MEMBER" digest="sha256:6b2abffcb7d53ec82da1f9f1f0ef2bc8dfe4ffd3fff37e45b0c9382b81f40e1a" -->

<!-- pdac:cite id="TERM-MEMBER-ROLE" digest="sha256:cc11a50db080d5fd2bbad00b3d42595ed9bac7b9201499da2c9afdff2e93f563" -->

<!-- pdac:cite id="SB-FAMILY-MEMBER-ADDED" digest="sha256:44f4ed434788ee330be3c8683ac1bac5de71d21a8de1a7cc713a93407bf01279" -->

<!-- pdac:cite id="SB-FAMILY-DUPLICATE-MEMBER-REJECTED" digest="sha256:bba14e81897a06d9fac45d5710b26eea027cf434618adeb74d47e48572064741" -->

<!-- pdac:cite id="SB-FAMILY-INVALID-ROLE-REJECTED" digest="sha256:fb3254e788b856fd2cb8ef03e8db535b0ff078b59b4853f48900131c0b5e843f" -->

<!-- pdac:cite id="SB-FAMILY-MEMBER-UNKNOWN-HOUSEHOLD-REJECTED" digest="sha256:c42ef6ffc1ecaf310c032100e99162d5e931036e861a9b8be02eb6c2066d499a" -->

#### Scenario: A member is added to the household

- GIVEN a household exists
- WHEN a member is added with a valid name and role
- THEN the member is added to the household with a unique MemberId
- AND a `MemberAdded` event is emitted

#### Scenario: A non-manager cannot add a member

- GIVEN a household exists
- AND the requesting member is not a manager
- WHEN that member attempts to add a person
- THEN the operation is rejected

#### Scenario: Only an adult may be designated as a manager

- WHEN a Child or Pet is added with the manager designation
- THEN the operation is rejected

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

<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:063379cd34cc983d03c1d12919164e3908c4584dffa4afc08907b0f5449dbac5" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:30c81e45dccad3cf83e793889e39e979c6e0d753ecf7fed1f41fd9c06f6cd0df" -->

<!-- pdac:cite id="BC-FAMILY" digest="sha256:1b48c9a4b2eba0dfb34314f7afbe1b5801e4f8c41e0ab5a586a9e7da57f7ee4f" -->

#### Scenario: A member is removed from the household

- GIVEN a household exists with at least one member
- WHEN that member is removed
- THEN the member is no longer part of the household roster
- AND a `MemberRemoved` event is emitted

---

### Requirement: Member Core Details Update

A household manager SHALL be able to update a member's full name, role, and optional birth date. A non-manager may update their own full name and birth date, but not their role. A birth date, when provided, must be in the past.

Members may edit their own profile details through a separate profile update path.

<!-- pdac:cite id="FR-FAMILY-UPDATE-MEMBER-DETAILS" digest="sha256:70f4398cbff6667b04a7eba73866734a1534e15e49e72bfd1f034e415b8288fc" -->

<!-- pdac:cite id="UC-FAMILY-UPDATE-MEMBER-DETAILS" digest="sha256:92f23ecfe017412c989248ea46ce2ec45929c8d67ec422db4cf3d36aa792fb6e" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:30c81e45dccad3cf83e793889e39e979c6e0d753ecf7fed1f41fd9c06f6cd0df" -->

<!-- pdac:cite id="BR-FAMILY-BIRTH-DATE-IN-PAST" digest="sha256:c25c71fbb27dfc096b85fd80d24bef2910fb48f95d6315fee2dbdcfa7d5e9565" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MANAGER" digest="sha256:9e4e0dad11e4c8cb94efdebca7ef64b3dd6c21db313e05dbadaaf7f3807075fd" -->

<!-- pdac:cite id="TERM-MANAGER" digest="sha256:a6debc0d573e79f332ebb681bdaa81c3dbe9616b2301579d004ac2e0d5c4f55b" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-UPDATES-MEMBER" digest="sha256:bb4775801d4e1c3dd9c4e44799d2e527d5ef1234d96ca3a0c4150223e5d1c7f1" -->

<!-- pdac:cite id="SB-FAMILY-NON-MANAGER-UPDATE-REJECTED" digest="sha256:712385fedaad18d8958e6244613d027090b275873f8e50aa212336b9ff96cbf8" -->

<!-- pdac:cite id="SB-FAMILY-FUTURE-BIRTH-DATE-REJECTED" digest="sha256:afd80ffb177ffddf17807754a37168e874a9d22138d7d089af8a7fc8cc3aecef" -->

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

#### Scenario: A non-manager updates their own name and birth date

- GIVEN a household member is not a manager
- WHEN they update their own name and birth date with valid values
- THEN those details are updated

#### Scenario: A non-manager cannot change their own role

- GIVEN a household member is not a manager
- WHEN they try to change their own role
- THEN the update is rejected
- AND their role is unchanged

#### Scenario: Birth date in the future is rejected

- WHEN a member update is submitted with a future birth date
- THEN the update is rejected
- AND a validation error is returned

---

### Requirement: Member Profile Update

A member SHALL be able to update their own optional contact details: preferred name, phone, email, and household note.

Managers may also update any member's profile details.

<!-- pdac:cite id="FR-FAMILY-UPDATE-MEMBER-PROFILE" digest="sha256:419440e8bcc608a59676f0c052ac846a101b07ff3a405544918164a9d3dc1394" -->

<!-- pdac:cite id="UC-FAMILY-UPDATE-MEMBER-DETAILS" digest="sha256:92f23ecfe017412c989248ea46ce2ec45929c8d67ec422db4cf3d36aa792fb6e" -->

<!-- pdac:cite id="TERM-MEMBER-PROFILE" digest="sha256:4f9eca4718b2c44620f39092dfda4752c728803b678fd3b138b1148ef16ea728" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MEMBER" digest="sha256:15b65c1c587d35c6e28a20522a70a619060adda71cdb22dc0851a3812a8e2ec4" -->

<!-- pdac:cite id="SB-FAMILY-MEMBER-UPDATES-OWN-PROFILE" digest="sha256:3da1acb0e59ff82769e4a1e2fb2ec20b48c1634627b17e53d44f7890b3b51e70" -->

#### Scenario: Member updates their own profile

- GIVEN a household member is authenticated
- WHEN that member updates their own preferred name, phone, email, or household note
- THEN the profile is updated

#### Scenario: Manager updates another member's profile

- GIVEN a manager views another household member's profile
- WHEN the manager updates that member's phone number
- THEN the profile shows the updated phone number

---

### Requirement: Member Access Provisioning

A household manager SHALL be able to invite a person using a copyable single-use link tied to that person's email, replace or revoke a pending link, and disable or re-enable application access. DomusMind SHALL leave credential recovery to the configured OIDC provider.

The person SHALL accept the invitation after authenticating with the configured OIDC provider and presenting a matching provider-verified email. Only then SHALL DomusMind bind the provider identity to the household person. Pets cannot receive an invitation.

The access state of a member is one of: `NoAccess`, `InvitedOrProvisioned`, `Active`, `Disabled`.

| State | Meaning |
|---|---|
| `NoAccess` | No invitation or provider identity is bound |
| `InvitedOrProvisioned` | A single-use invitation link is pending; no provider identity is bound |
| `Active` | A provider identity is bound and application access is enabled |
| `Disabled` | The provider identity remains bound but application access is denied |

Rules:
- Only a manager may invite, replace or revoke a pending link, and disable or re-enable access for other members
- A manager may not disable their own access
- Pets cannot receive access
- A replaced, revoked or used link cannot be accepted
- Disabling access denies existing and new DomusMind sessions without changing the provider account

<!-- pdac:cite id="UC-FAMILY-ACCEPT-ACCESS-INVITATION" digest="sha256:5d6d3a380233f8af29fa1123cb5d826315ac7161fa7f410b9510557039965088" -->
<!-- pdac:cite id="UC-FAMILY-MANAGE-INVITATION" digest="sha256:05454d9ba1eddb527901ff4b00f47d7f02e60be1543ba00838f42c32a09f5e29" -->
<!-- pdac:cite id="BR-OIDC-IDENTITY-BOUND-TO-ACCESS" digest="sha256:6c34260aff83b573ced0b3f499f0d119fff78dd623b71b901d0db73f0b6a712a" -->

<!-- pdac:cite id="FR-FAMILY-MANAGE-MEMBER-ACCESS" digest="sha256:47e0ea395954818d06db2c72cd7fee5b8f50cc24080212297796936e7f97edce" -->

<!-- pdac:cite id="UC-FAMILY-PROVISION-MEMBER-ACCESS" digest="sha256:1eddf2b7a223190aeaecb11ca414ea41cfd8db45eaf8603632e567d7eb80e927" -->

<!-- pdac:cite id="UC-FAMILY-DISABLE-MEMBER-ACCESS" digest="sha256:39a4cdac86168482a1a1a2cdb7e6d0443de293110d29a23fc2a6d91647ab52f5" -->

<!-- pdac:cite id="UC-FAMILY-ENABLE-MEMBER-ACCESS" digest="sha256:9b11a6d17c359c8d206298a4e0e760012b71bcbd67c248cf56f6db7d32134bd3" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:30c81e45dccad3cf83e793889e39e979c6e0d753ecf7fed1f41fd9c06f6cd0df" -->

<!-- pdac:cite id="BR-FAMILY-MANAGER-KEEPS-OWN-ACCESS" digest="sha256:faa4e77941164ccae0e3a5ad45d8ba3ec3712a2d6b0179c8a4cbbb0352f41b1d" -->

<!-- pdac:cite id="BR-FAMILY-PET-RESTRICTIONS" digest="sha256:7cb38e97316b489c3507fbbfaa456247122efb0c220d01fa0787024ce0ba9efe" -->

<!-- pdac:cite id="TERM-MEMBER-ACCESS" digest="sha256:0070e81c6cc9c4e9bf23c261719710c20db7563eaf584782092918ca26137de7" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MANAGER" digest="sha256:9e4e0dad11e4c8cb94efdebca7ef64b3dd6c21db313e05dbadaaf7f3807075fd" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-PROVISIONS-ACCESS" digest="sha256:4cc7900ef666f595a8f3f3f0bfc65fb7cb427985f89141066feaeaf8bbd4860a" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-DISABLES-ACCESS" digest="sha256:b80811021daa3ee3159ecfe532fe2cb12acaa7acaa4b1ab040b2f29440453f7a" -->

<!-- pdac:cite id="SB-FAMILY-MANAGER-ENABLES-ACCESS" digest="sha256:b0d6d518e4d13135a03d682a8897691b49ce06b12a90e6d1f87820402130738a" -->

<!-- pdac:cite id="SB-FAMILY-PET-ACCESS-REJECTED" digest="sha256:8c88a669e58064ed8ce4aa0b14b32470a402b7d8cde3c964a19a69b35705460b" -->

#### Scenario: Manager provisions access for a member

- GIVEN a member exists with `NoAccess` state
- AND the requesting user is a manager
- AND the member role is not Pet
- WHEN the manager creates an invitation link
- THEN the manager can copy the single-use link
- AND no provider identity is yet bound
- AND the member state becomes `InvitedOrProvisioned`

#### Scenario: A non-manager cannot administer access

- GIVEN a member exists
- AND the requesting user is not a manager
- WHEN they attempt to invite, replace or revoke an invitation, disable, or enable access
- THEN the operation is rejected

#### Scenario: Manager disables a member's access

- GIVEN a member exists with `Active` state
- AND the requesting user is a manager
- AND the target is not the requesting manager themselves
- WHEN access is disabled
- THEN the member state becomes `Disabled`
- AND existing DomusMind sessions cannot use the household

#### Scenario: A manager cannot disable their own access

- GIVEN the requesting user is a manager with active access
- WHEN they attempt to disable their own access
- THEN the operation is rejected
- AND their access remains active

#### Scenario: An invited person accepts access

- GIVEN a person has a valid pending invitation link
- AND their configured OIDC provider reports a verified email matching the invitation
- WHEN that person accepts the link
- THEN the provider identity is bound to that household person
- AND their access becomes `Active`
- AND the link cannot be used again

#### Scenario: A manager replaces a pending invitation link

- GIVEN a person has a pending invitation link
- AND the requesting user is a manager
- WHEN the manager replaces that link
- THEN the old link can no longer be accepted
- AND the manager can copy a new single-use link for the same person and email

#### Scenario: A manager revokes a pending invitation link

- GIVEN a person has a pending invitation link
- AND the requesting user is a manager
- WHEN the manager revokes that link
- THEN the link can no longer be accepted
- AND the person's access becomes `NoAccess`

#### Scenario: Manager enables a member's access

- GIVEN a member exists with `Disabled` state
- AND the requesting user is a manager
- WHEN access is enabled
- THEN the member state becomes `Active`
- AND the existing provider identity binding is retained

#### Scenario: Provisioning a Pet is rejected

- GIVEN a member with role `Pet`
- WHEN access provisioning is attempted
- THEN the operation is rejected

---

### Requirement: Pet Registration

A household SHALL be able to register a pet by adding a member with `MemberRole = Pet`.

In Phase 1, pets are stored as `FamilyMember` entries with the `Pet` role — not as a distinct Pet entity. The domain design describes Pet as a separate entity; Phase 1 collapses this into the member model (see NOTE N6).

Pets in V1:
- may be registered only by a household manager
- cannot be provisioned with system access
- cannot receive task assignments
- cannot hold responsibility ownership
- are excluded from Agenda household rows and member coordination timelines
- may be added as event participants (e.g. vet appointments)
- appear in the member directory as a distinct group after adults and children

<!-- pdac:cite id="FR-FAMILY-REGISTER-PET" digest="sha256:b8c358e73810cc515d425504148ec62318245b1a6bcf1b320267fd5e8b16c104" -->

<!-- pdac:cite id="UC-FAMILY-REGISTER-PET" digest="sha256:004a2cdf0e5d6371a6a491cab790e79ad12ba3a3b7563366f6b646f3ac743958" -->

<!-- pdac:cite id="BR-FAMILY-PET-RESTRICTIONS" digest="sha256:7cb38e97316b489c3507fbbfaa456247122efb0c220d01fa0787024ce0ba9efe" -->

<!-- pdac:cite id="TERM-PET" digest="sha256:16bebdff01978d3c6944ba1d53cd4eeca2decdce19ff5dec6974d5bd28aebea9" -->

<!-- pdac:cite id="BR-FAMILY-DIRECTORY-ORDER" digest="sha256:ba044177544fe96f1ba1690c992c4b5ecdf6a8b0757f078b27094cf9661049d6" -->

<!-- pdac:cite id="SB-FAMILY-PET-ADDED" digest="sha256:5b27a9005f13cb148c7ce0720e07b6c145e3522351fb931c0fb2152f7a67988d" -->

<!-- pdac:cite id="SB-FAMILY-PET-ACCESS-REJECTED" digest="sha256:8c88a669e58064ed8ce4aa0b14b32470a402b7d8cde3c964a19a69b35705460b" -->

#### Scenario: A pet is added to the household

- GIVEN a household exists
- WHEN a member is added with a valid name and `MemberRole = Pet`
- THEN the pet is part of the household with a unique MemberId
- AND a `MemberAdded` event is emitted

#### Scenario: A pet may participate in a plan but cannot coordinate work

- GIVEN a household has a registered pet
- WHEN planning, task assignment, area ownership, and household Agenda rows are considered
- THEN the pet may be selected as a plan participant
- AND the pet cannot be a task assignee or area owner
- AND the pet has no household Agenda row or member timeline

#### Scenario: A non-manager cannot register a pet

- GIVEN a household exists
- AND the requesting member is not a manager
- WHEN they attempt to register a pet
- THEN the operation is rejected

#### Scenario: A pet is removed from the household

- GIVEN a household exists with a registered pet
- WHEN the pet member is removed
- THEN the pet is no longer part of the household
- AND a `MemberRemoved` event is emitted

---

### Requirement: Relationship Assignment _(V1.1)_

> **Scope note:** This capability is explicitly deferred to V1.1 in `specs/system/system-spec.md`. Relationship semantics are modeled in the domain but not exposed via API or UI in V1.

A household SHALL be able to define a structural relationship between two of its members.

Relationships connect two distinct members of the same household. Relationship types and management behavior remain deferred with this V1.1 capability.

<!-- pdac:cite id="BC-FAMILY" digest="sha256:1b48c9a4b2eba0dfb34314f7afbe1b5801e4f8c41e0ab5a586a9e7da57f7ee4f" -->

<!-- pdac:cite id="BR-FAMILY-MANAGERS-ADMINISTER" digest="sha256:30c81e45dccad3cf83e793889e39e979c6e0d753ecf7fed1f41fd9c06f6cd0df" -->

<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:063379cd34cc983d03c1d12919164e3908c4584dffa4afc08907b0f5449dbac5" -->

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

The directory is ordered: Adults first, then Children, then Pets. Within each group, managers appear before other members; within that ordering, entries are alphabetical by name.

The directory includes server-computed access flags. Clients must not re-derive permission or access state from raw fields.

<!-- pdac:cite id="FR-FAMILY-MEMBER-DIRECTORY" digest="sha256:543c87a84a5ed1f97abdb2f19498adcc5b43b68f2a491e96681e9313bb8d5f36" -->

<!-- pdac:cite id="UC-FAMILY-VIEW-MEMBER-DIRECTORY" digest="sha256:d176dcfdbbfc80a27ec30322a6051462125b0c85b7f231a1ddc59ba96502d631" -->

<!-- pdac:cite id="BR-FAMILY-DIRECTORY-ORDER" digest="sha256:ba044177544fe96f1ba1690c992c4b5ecdf6a8b0757f078b27094cf9661049d6" -->

<!-- pdac:cite id="TERM-MEMBER-DIRECTORY" digest="sha256:f93bcde0d68fc37932221b95b153b602d3a68ac3b72c119f0d277518a3c7de0f" -->

<!-- pdac:cite id="ACT-HOUSEHOLD-MEMBER" digest="sha256:15b65c1c587d35c6e28a20522a70a619060adda71cdb22dc0851a3812a8e2ec4" -->

#### Scenario: Directory entries are shown in the defined order

- GIVEN a household has adults, children, and pets
- WHEN a household member views the directory
- THEN adults appear first, then children, then pets
- AND managers appear before other members in each group, followed by alphabetical name order

#### Scenario: Directory access actions reflect the viewer and target

- GIVEN a household member views the directory
- WHEN directory entries are returned
- THEN the server-computed access state and permitted actions are shown for each entry
- AND a non-manager is not offered access administration
- AND a manager is offered access provisioning only for eligible members without access

---

### Requirement: Household Settings

A household manager SHALL be able to change the household's name, primary language, first day of week, and date format. These settings apply to everyone in the household. A person who is not a manager cannot change them.

#### Scenario: Manager changes household settings

- GIVEN a household exists
- AND the requesting member is a manager
- WHEN the manager changes one or more household settings
- THEN the selected settings are applied to the household

#### Scenario: Non-manager cannot change household settings

- GIVEN a household exists
- AND the requesting member is not a manager
- WHEN they attempt to change a household setting
- THEN the operation is rejected

#### Scenario: First day of week applies to Agenda

- GIVEN a household's first day of week is Monday
- WHEN a member views an Agenda week
- THEN the week starts on Monday

#### Scenario: Household rename is shared

- WHEN a manager changes the household name
- THEN the new name is shown to household members

---

### Requirement: Identity Boundary Enforcement

The Family context SHALL be the sole owner of household structure.

No other context may create, modify, or remove members, pets, or relationships. Other contexts reference family entities by ID only. Cross-context reactions happen through domain events.

<!-- pdac:cite id="FR-FAMILY-IDENTITY-BOUNDARY" digest="sha256:0cc09f6a5c86dffaa94214e9272fad2e47097d70df0f8a058d42aa8909a4f6f6" -->

<!-- pdac:cite id="BR-FAMILY-OWNS-HOUSEHOLD-STRUCTURE" digest="sha256:063379cd34cc983d03c1d12919164e3908c4584dffa4afc08907b0f5449dbac5" -->

<!-- pdac:cite id="BC-FAMILY" digest="sha256:1b48c9a4b2eba0dfb34314f7afbe1b5801e4f8c41e0ab5a586a9e7da57f7ee4f" -->

#### Scenario: Cross-context activity does not change household identity

- WHEN a person is assigned a task, owns an area, or joins a plan
- THEN their household identity and details remain unchanged

#### Scenario: Other contexts cannot change household structure

- WHEN planning, tasks, areas, or lists operate on household data
- THEN they refer to household people by identity
- AND provide no operation to add, change, or remove people, pets, or relationships

---

## Domain Events

| Event | Emitted when |
|---|---|
| `FamilyCreated` | A new household is created |
| `MemberAdded` | A member is added to the household |
| `MemberRemoved` | A member is removed from the household |
| `MemberAdded` (role=Pet) | A pet is registered as a household member |
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

**N1 — Member roles**: The role set is Adult, Child, and Pet. There is no Caregiver role (decided: Q-0021). A child is a member with the Child role.

**N2 — Household settings**: The baseline household settings are its name, primary language, first day of week, and date format. Managers may change them; changes apply household-wide (decided: Q-0025).

**N3 — Manager self-removal and last-manager constraint**: No source document specifies whether a manager can remove themselves, whether a household must always have at least one manager, or what happens when the last manager is removed. Excluded pending clarification.

**N4 — Manager designation**: The person who creates a household is its first manager; only adults can be managers (decided: Q-0022). Managers may designate an adult as a manager when adding or editing that adult. The product model does not define a separate manager administration capability.

**N5 — Relationship assignment deferral**: `specs/system/system-spec.md` explicitly defers `assign-relationship` and `remove-member` to V1.1, citing cascading complexity. The relationship model (Relationship entity, RelationshipType, RelationshipAssigned/Removed events) is fully modeled in the domain but not exposed via V1 API or UI.

**N6 — Pet representation**: A pet is a household member with the Pet role, shares the member identity space, and uses the member lifecycle. Pet registration is manager-only (decided: Q-0024).

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
