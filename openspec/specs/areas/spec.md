<!-- pdac-scope: cited -->

# Areas Specification

## Purpose

Areas represent household accountability structure. Each Area is a named domain of household management — such as school, food, finances, or maintenance — with explicit ownership assigned to family members.

The Areas surface answers: which household areas exist, who owns each one, who supports each one, and where ownership is missing.

Areas model accountability, not execution. Owning an Area means being accountable for that domain of household life; it does not imply direct execution of every related task, plan, or routine.

Areas are scoped to the household (family). Other capabilities — tasks, plans, routines, calendar events — may reference an Area as organizational context, but only the Responsibilities context may change ownership assignments.

In the domain model, an Area is a **Responsibility Domain**. In the product surface and household-facing language, it is an **Area**. The product roles **Owner** and **Support** correspond to the domain roles **Primary Owner** and **Secondary Owner**.

---

## Requirements

### Requirement: Area Creation

A household SHALL be able to create a named Area scoped to the family.

An Area name is required. Ownership is optional at creation — an Area may exist without an assigned owner.

Other capabilities (tasks, plans, meal plans) may optionally reference an Area for categorization and accountability context. Sparse usage is acceptable; the household functions without needing every Area to be fully configured.

Default Areas may be bootstrapped for a newly created household to reduce setup friction.

<!-- pdac:cite id="FR-AREAS-CREATE-AREA" digest="sha256:4c5286b209258a4917ec0adddd3cb73c86b3d295ad5f8ad07bfb776af5031428" -->

<!-- pdac:cite id="UC-AREAS-CREATE-AREA" digest="sha256:27123ebf2580c2346b975a75a78ce1ef5934c3f07561e1535ca1481202e5cfb2" -->

<!-- pdac:cite id="BR-AREAS-NAME-REQUIRED" digest="sha256:3bb588f6d5de580a64bc454d23ce3958b5d786983350ccfc40370fcdc1d1d834" -->

<!-- pdac:cite id="BR-AREAS-OWNERSHIP-OPTIONAL" digest="sha256:eb1cd98a004a9c552021c8c00b7bd0c9f120c8d46ae8a6e8768ea12e54b00dc7" -->

<!-- pdac:cite id="BR-AREAS-ONE-HOUSEHOLD" digest="sha256:6aad55f75c6b2324feb22b98da2885b7f081362f59a9eaa4b6d2b08da8dd5d89" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:f76c31e9c4623b245b4e7bea9fe81efbead0b3531e3877656e1f29c284ec809c" -->

<!-- pdac:cite id="TERM-AREA" digest="sha256:f92e65449cfc1d379e15a6db5a40c2b069c68ea2671e45622650cb52d6354889" -->

<!-- pdac:cite id="SB-AREAS-CREATE-AREA" digest="sha256:11fdc9fe2fac5f6cc6f28ecb4b826d6a9c2ba273d77b94f862aef9122db07aff" -->

<!-- pdac-drift ids="BC-RESPONSIBILITIES" summary="spec allows default Areas to be bootstrapped for a new household; the model has no default Areas (decided: Q-0015)" -->

<!-- pdac-drift ids="BR-AREAS-OWNERSHIP-EXCLUSIVE, TERM-AREA, BC-RESPONSIBILITIES" summary="spec lists meal plans among capabilities that may reference an Area; the model says meal plans do not reference Areas (decided: Q-0040)" -->

#### Scenario: Household creates an Area

- GIVEN a family exists
- WHEN the household creates an Area with a valid name
- THEN the Area is created scoped to that family
- AND it has no owner assigned by default

---

### Requirement: Primary Owner Assignment

An Area SHALL have at most one primary owner at any time.

The primary owner is the member accountable for that Area. The primary owner must belong to the same family as the Area.

Assigning a primary owner when one is already set replaces the previous owner. Transfer of primary ownership must always result in exactly one active primary owner.

<!-- pdac:cite id="FR-AREAS-ASSIGN-OWNER" digest="sha256:c31f87b9eed954ff87bfb992268d8021997892344a604f18b3094a4435cbceb8" -->

<!-- pdac:cite id="UC-AREAS-ASSIGN-OWNER" digest="sha256:bf9cc076c8daced2bfa0edbe80064c51d5f6f6643bbe5bd4b2737e316a52fe65" -->

<!-- pdac:cite id="BR-AREAS-SINGLE-OWNER" digest="sha256:2123ead862639b7e83626f13b76c836f07f6f1dee1c64ecc41358717d84c35b4" -->

<!-- pdac:cite id="BR-AREAS-SAME-HOUSEHOLD-PEOPLE" digest="sha256:fe9903e5ef4853ddbc2427015d9291f8b556755c63f87f7c2715dd50a94fbc2d" -->

<!-- pdac:cite id="BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE" digest="sha256:aeb3d7e1e4a05d4a238fd2ca801118605302d9420859397ccae4bd7ef8baa9fc" -->

<!-- pdac:cite id="BR-AREAS-OWNER-NOT-SUPPORT" digest="sha256:c39161b43a6c0b8a56cf8f1e270af037ccd76a0d55f4668395f2b93b91c24677" -->

<!-- pdac:cite id="BR-AREAS-NO-INACTIVE-ASSIGNEE" digest="sha256:658f15559d2d4159e0aaa9f34fbb95487cd63f464d8fc258946cd02e8cd9e174" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:f76c31e9c4623b245b4e7bea9fe81efbead0b3531e3877656e1f29c284ec809c" -->

<!-- pdac:cite id="TERM-AREA-OWNER" digest="sha256:e8fd632c146effbb577bc5e063a7c1684f46e5602682d823f7bdeb5ff60e4558" -->

<!-- pdac:cite id="SB-AREAS-ASSIGN-FIRST-OWNER" digest="sha256:3fc0f6fcad277c6d19bc556f7fd0a11ddedfc4c99b414d893c7ccd293741e9fb" -->

<!-- pdac:cite id="SB-AREAS-REPLACE-OWNER" digest="sha256:3dd014700ead38f6b8fc85fb816dc7baecdd437bb129f928f39f4bbfe9bbb04f" -->

<!-- pdac:cite id="SB-AREAS-REJECT-OUTSIDE-OWNER" digest="sha256:8bec78040b75f7fa49b04a8451b147c42145235669eade94bdaf88d4d49aa3e0" -->

#### Scenario: Household assigns a primary owner to an Area

- GIVEN an Area exists with no primary owner
- AND a family member exists
- WHEN the household assigns that member as the primary owner
- THEN the member becomes the primary owner of the Area
- AND the Area moves from an unowned to an owned state

#### Scenario: Primary owner is replaced

- GIVEN an Area already has a primary owner
- WHEN the household assigns a different member as the primary owner
- THEN the new member becomes the primary owner
- AND the previous primary owner no longer holds that role

#### Scenario: Non-family member cannot be assigned as owner

- GIVEN a member that does not belong to the family
- WHEN the household attempts to assign that member as primary owner of a family Area
- THEN the assignment is rejected

---

### Requirement: Secondary Owner Assignment

An Area MAY have one or more secondary owners.

Secondary owners provide backup or shared accountability coverage for the Area. Each secondary owner must belong to the same family as the Area. Secondary owners must be unique within the Area — the same member cannot be assigned as secondary owner more than once.

A secondary owner does not replace the primary owner.

<!-- pdac:cite id="FR-AREAS-ADD-SUPPORT" digest="sha256:b375cece8091243562c3f8b85f7aeb03d789a2b7f1a93984d6fdb7e00403e2cb" -->

<!-- pdac:cite id="UC-AREAS-ADD-SUPPORT" digest="sha256:a406223e1b237d39cc41285d3a25add74ff82ff72e5993e6aa53ec933a0eda41" -->

<!-- pdac:cite id="BR-AREAS-UNIQUE-SUPPORT" digest="sha256:9d14a0933acc6831e63b794111f7f6eb5671dff483c0e4eb0f8008dd8484ca66" -->

<!-- pdac:cite id="BR-AREAS-SAME-HOUSEHOLD-PEOPLE" digest="sha256:fe9903e5ef4853ddbc2427015d9291f8b556755c63f87f7c2715dd50a94fbc2d" -->

<!-- pdac:cite id="BR-AREAS-NO-INACTIVE-ASSIGNEE" digest="sha256:658f15559d2d4159e0aaa9f34fbb95487cd63f464d8fc258946cd02e8cd9e174" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:f76c31e9c4623b245b4e7bea9fe81efbead0b3531e3877656e1f29c284ec809c" -->

<!-- pdac:cite id="TERM-AREA-SUPPORT" digest="sha256:57f25b36298732ba7f37c884da6113dc98e57a3490316d2f72d48e3f0fdb53d6" -->

<!-- pdac:cite id="SB-AREAS-ADD-SUPPORT" digest="sha256:bfcdabea7622ac92c4079ca31cd06fe9c681e16baf2d39e50659925c2896e983" -->

<!-- pdac:cite id="SB-AREAS-REJECT-DUPLICATE-SUPPORT" digest="sha256:bbcdbc753f9109215be8405ed7b1223b90b148a7c0d8ea8c41d96fb67cea8679" -->

<!-- pdac-drift ids="BR-AREAS-OWNER-NOT-SUPPORT, FR-AREAS-ADD-SUPPORT, SB-AREAS-REJECT-OWNER-AS-SUPPORT" summary="spec accepts as Support any family member not already Support; the model rejects adding the Area's Owner as Support (decided: Q-0017)" -->

#### Scenario: Household adds a secondary owner to an Area

- GIVEN an Area exists
- AND a family member is not already a secondary owner of that Area
- WHEN the household assigns the member as a secondary owner
- THEN the member is added to the secondary owner set
- AND the primary owner is unchanged

#### Scenario: Duplicate secondary owner is rejected

- GIVEN a member is already a secondary owner of an Area
- WHEN the household attempts to assign the same member as a secondary owner again
- THEN the assignment is rejected

---

### Requirement: Secondary Owner Removal

A household SHALL be able to remove a secondary owner from an Area.

Removing a secondary owner does not affect the primary owner or other secondary owners.

<!-- pdac:cite id="FR-AREAS-REMOVE-SUPPORT" digest="sha256:8f3c373489f138e604669eec09d3e8669cd95ab76a163b5485f449fb6ca39aa7" -->

<!-- pdac:cite id="UC-AREAS-REMOVE-SUPPORT" digest="sha256:8c121adf917654287c5d0cb4d9e680f7bce2eda254fea1ceaf0f0eed544fd6ba" -->

<!-- pdac:cite id="BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE" digest="sha256:aeb3d7e1e4a05d4a238fd2ca801118605302d9420859397ccae4bd7ef8baa9fc" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:f76c31e9c4623b245b4e7bea9fe81efbead0b3531e3877656e1f29c284ec809c" -->

<!-- pdac:cite id="TERM-AREA-SUPPORT" digest="sha256:57f25b36298732ba7f37c884da6113dc98e57a3490316d2f72d48e3f0fdb53d6" -->

<!-- pdac:cite id="SB-AREAS-REMOVE-SUPPORT" digest="sha256:a9baf854d055e97217e37ce50e93be0b03ddfa6eba4a908314843e05db358ef7" -->

#### Scenario: Household removes a secondary owner

- GIVEN an Area has at least one secondary owner
- WHEN the household removes one of the secondary owners
- THEN that member is no longer a secondary owner of the Area
- AND all other secondary owners remain unchanged
- AND the primary owner is unchanged

---

### Requirement: Responsibility Transfer

A household SHALL be able to explicitly transfer primary ownership of an Area to another member.

Transfer is an explicit, auditable operation — not an implicit overwrite. The Area remains active and owned throughout the transfer. After transfer, exactly one active primary owner exists.

The new primary owner must belong to the same family as the Area.

<!-- pdac:cite id="FR-AREAS-TRANSFER-OWNERSHIP" digest="sha256:48882b866db8bc9088a6a713bb6a625a2c05b94d9b1554c0ea765b7ef61ea493" -->

<!-- pdac:cite id="UC-AREAS-ASSIGN-OWNER" digest="sha256:bf9cc076c8daced2bfa0edbe80064c51d5f6f6643bbe5bd4b2737e316a52fe65" -->

<!-- pdac:cite id="BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE" digest="sha256:aeb3d7e1e4a05d4a238fd2ca801118605302d9420859397ccae4bd7ef8baa9fc" -->

<!-- pdac:cite id="BR-AREAS-SAME-HOUSEHOLD-PEOPLE" digest="sha256:fe9903e5ef4853ddbc2427015d9291f8b556755c63f87f7c2715dd50a94fbc2d" -->

<!-- pdac:cite id="BR-AREAS-SINGLE-OWNER" digest="sha256:2123ead862639b7e83626f13b76c836f07f6f1dee1c64ecc41358717d84c35b4" -->

<!-- pdac:cite id="TERM-RESPONSIBILITY-TRANSFER" digest="sha256:2c12c27419fdf795b4b04949b2a3bf85179682a621310d30593f8ab8d4360757" -->

<!-- pdac:cite id="SB-AREAS-TRANSFER-OWNERSHIP" digest="sha256:10ba20534afdd2707eca9a31b48cb0077911d9920e2dae9500f33e8727ac70ff" -->

<!-- pdac:cite id="SB-AREAS-REJECT-OUTSIDE-TRANSFER" digest="sha256:60fbdf8fc9da15116d77853031f8cc62aa3d6bf644a5455606049f7db86b78d1" -->

#### Scenario: Household transfers Area ownership

- GIVEN an Area has a primary owner
- AND another family member exists
- WHEN the household transfers primary ownership to the new member
- THEN the new member becomes the primary owner
- AND the previous primary owner no longer holds the primary owner role
- AND the Area remains active

#### Scenario: Transfer to a non-family member is rejected

- GIVEN a member that does not belong to the family
- WHEN the household attempts to transfer primary ownership to that member
- THEN the transfer is rejected
- AND the existing primary owner is unchanged

---

### Requirement: Area Renaming

A household SHALL be able to rename an Area.

<!-- pdac:cite id="FR-AREAS-RENAME-AREA" digest="sha256:a6386ea947e5f888d6a4d1c82775e3b29b0033023df682f4b079820a9d1744d9" -->

<!-- pdac:cite id="UC-AREAS-RENAME-AREA" digest="sha256:c3289681b13442b610865cb4baa631ad38f8cf53aeffe432a1144b1f25dc49b2" -->

<!-- pdac:cite id="BR-AREAS-NAME-REQUIRED" digest="sha256:3bb588f6d5de580a64bc454d23ce3958b5d786983350ccfc40370fcdc1d1d834" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:f76c31e9c4623b245b4e7bea9fe81efbead0b3531e3877656e1f29c284ec809c" -->

<!-- pdac:cite id="SB-AREAS-RENAME-AREA" digest="sha256:4e62bd6de8def338ddd13a93b7c9c3237375c87856b0c4978f356a0dbb362f5c" -->

#### Scenario: Household renames an Area

- GIVEN an Area exists
- WHEN the household provides a new name for the Area
- THEN the Area is updated with the new name

---

### Requirement: Area Archiving

A household SHALL be able to archive an Area.

Archived Areas are retained and visible when filtered for, but are distinguished from active Areas.

<!-- pdac:cite id="FR-AREAS-ARCHIVE-AREA" digest="sha256:37a18941dff1c63714b9850f75311a250443fabcb729a5ac4d6ba21ff1c089bd" -->

<!-- pdac:cite id="UC-AREAS-ARCHIVE-AREA" digest="sha256:d773dcd1a024bc46c41aab2b1486c9e0b4e94480abbc4c2230a1ff56101b0ec6" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:f76c31e9c4623b245b4e7bea9fe81efbead0b3531e3877656e1f29c284ec809c" -->

<!-- pdac:cite id="BR-TASKS-ARCHIVED-AREA-CLEARED" digest="sha256:4ebf5122a85fc47a1b6ef5fecc66c463ac6a46846ae7fe4485d2097a97e42d7e" -->

<!-- pdac:cite id="SB-AREAS-ARCHIVE-AREA" digest="sha256:9f1c6bbcefc77d03bde3672550398eaf8633d6b3225d864f08ff9a4f21b55736" -->

#### Scenario: Household archives an Area

- GIVEN an Area exists
- WHEN the household archives it
- THEN the Area is marked as archived
- AND it is excluded from the default active view
- AND it remains visible when the archived filter is applied

---

### Requirement: Ownership Visibility

The Areas surface SHALL surface ownership gaps visibly.

The default ordered view prioritizes unowned Areas first, then partially assigned Areas, then fully assigned Areas, then archived Areas. This ordering keeps accountability gaps in view without requiring the household to search for them.

An Area is considered unowned when no primary owner is assigned. An Area is considered partially assigned when a primary owner is assigned but no secondary owner exists (this distinction is informational, not a hard system state).

Each Area row shows the Area name, primary owner (or a gap indicator if unowned), and support members if present.

<!-- pdac:cite id="FR-AREAS-OWNERSHIP-VISIBILITY" digest="sha256:87da19a0776edd63c305dbcc30a7e51ec98fe949c762e217df6f17d284c745b4" -->

<!-- pdac:cite id="UC-AREAS-REVIEW-OWNERSHIP" digest="sha256:fe5c941fe5760bc287bf7e51f2e59e21e0215853396e8f5760ada0ae99d35acd" -->

<!-- pdac:cite id="TERM-OWNERSHIP-GAP" digest="sha256:e403011bc8751c3eb99fa08906c90b4172f9e0f8e7eb8d4e49761a40ac843531" -->

<!-- pdac:cite id="JRN-AREAS-CLOSE-OWNERSHIP-GAPS" digest="sha256:dad23263cf93824de985037fd93456c2f1c335383145f25c10529c6251c3a396" -->

<!-- pdac:cite id="QR-AREAS-GAPS-AT-A-GLANCE" digest="sha256:f1a55dc43455210a0ed6dd85f11f80d67e68d984acd0d84e65e8f392fce5829f" -->

<!-- pdac:cite id="SB-AREAS-UNOWNED-FIRST" digest="sha256:9c21be8ac399f55a749f9b93986be95559b578f293922b375be1768afff351be" -->

<!-- pdac:cite id="SB-AREAS-GAP-INDICATOR" digest="sha256:797e4f31f1fca1a42437dcc02b8d5cf61763383fe4d34fb9e1f5a2e5e692a9e4" -->

#### Scenario: Unowned Area appears at the top of the list

- GIVEN a household has a mix of owned and unowned Areas
- WHEN the household views the Areas surface
- THEN unowned Areas appear before owned Areas in the default view

#### Scenario: Ownership gap is indicated on the Area row

- GIVEN an Area with no primary owner
- WHEN the household views the Areas list
- THEN a visible gap indicator is shown in place of the owner

---

### Requirement: Cross-Context Referencing

Other household capabilities — including tasks, plans, routines, and calendar events — MAY reference an Area for organizational context.

Referencing an Area does not change ownership assignments. Ownership belongs exclusively to the Responsibilities context. Other contexts use Area identity as a categorization anchor only.

<!-- pdac:cite id="FR-AREAS-CROSS-CONTEXT-REFERENCE" digest="sha256:5aff86faf538e5a0537325653933c5e4ef6edf40f4972323b71613dbcca78bd9" -->

<!-- pdac:cite id="BR-AREAS-OWNERSHIP-EXCLUSIVE" digest="sha256:f8cd1f144ecd083696ccb24518d4fe73ad467205b4e87883df7247db42a570f9" -->

<!-- pdac:cite id="BR-TASKS-AREA-CATEGORISES-ONLY" digest="sha256:9c2fb9c31feb96ab6309a5e768f3bfd87b7d9f028fe5a27e42accaf363a1b916" -->

<!-- pdac:cite id="BC-RESPONSIBILITIES" digest="sha256:288de55e6b2023b73e2a4c9992f9390250c4a7690500cd4d6a207c432f324cc5" -->

<!-- pdac:cite id="SB-AREAS-TASK-REFERENCES-AREA" digest="sha256:40e939b27933d2c4a62502023bc5b6a339b645184f3181e88e5a261419e6b197" -->

#### Scenario: A task references an Area

- GIVEN a task is created with a reference to an Area
- THEN the Area context is recorded on the task as a categorization reference
- AND the Area's ownership assignments are not affected

---

## Notes

### Terminology convergence

"Area" (product surface) and "Responsibility Domain" (domain model) refer to the same concept. All repository sources are consistent on this mapping. "Owner" maps to Primary Owner; "Support" maps to Secondary Owner. This spec uses Area, Owner, and Support when describing product-facing behavior, and notes the domain terms where relevant.

`docs/_legacy/04_contexts/areas.md` does not exist in this repository. The context document for this domain lives at `docs/_legacy/04_contexts/responsibilities.md`. The Areas label is used by the product surface (`00_product/surfaces/areas.md`) and the navigation entry point.

### AssignPrimaryOwner vs. TransferResponsibility

Both operations result in a new primary owner. The distinction documented in the source files is: direct assignment (when setting an owner for the first time or implicitly replacing one) versus explicit transfer (an intentional, auditable change of accountability between two named members). The behavioral difference in terms of system outcome is not fully specified. This should be clarified before implementing both commands.

<!-- pdac-drift ids="FR-AREAS-TRANSFER-OWNERSHIP, FR-AREAS-ASSIGN-OWNER, BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE, TERM-RESPONSIBILITY-TRANSFER" summary="spec leaves assignment vs transfer unresolved; the model makes them one household action where changing an existing Owner is a traceable transfer (decided: Q-0012)" -->

### Participant role

The domain model defines a Participant role (a member involved in an Area but not primarily accountable). Commands for adding and removing participants exist in the domain. However, the surface spec does not expose participants as a first-class product element — participant detail is described as "secondary" and shown only in the inspector or secondary metadata. No feature spec exists for participant management. This spec does not include a Participant requirement; it should be added when a feature spec is created.

### Area color

The surface spec describes an Area color cue (colored dot, changeable inline via the inspector). This is referenced in the Areas surface spec but is absent from the domain model and all feature specs. It is not included as a behavioral requirement here. It should be specified as a feature before implementation.

<!-- pdac-drift ids="FR-AREAS-AREA-COLOR, UC-AREAS-CHANGE-AREA-COLOR, TERM-AREA" summary="spec excludes the Area colour cue as unspecified; the model makes the colour cue part of the product (decided: Q-0019)" -->

### Archive: no removal behavior specified

The repository specifies archiving but does not document reactivation or permanent deletion of Areas. `ReactivateResponsibilityDomain` is listed as a future command in the context document. This spec covers archiving only.

---

## Source References

- `docs/_legacy/04_contexts/responsibilities.md`
- `00_product/surfaces/areas.md`
- `specs/features/responsibilities/create-responsibility-domain.md`
- `specs/features/responsibilities/assign-primary-owner.md`
- `specs/features/responsibilities/assign-secondary-owner.md`
- `specs/features/responsibilities/transfer-responsibility.md`
- `docs/_legacy/03_domain/ubiquitous-language.md`
