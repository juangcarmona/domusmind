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

<!-- pdac:cite id="FR-AREAS-CREATE-AREA" digest="sha256:e47160cbf20bcebd66f90e8782f13ebe117faed58218a549f3373dbec29c7db1" -->

<!-- pdac:cite id="UC-AREAS-CREATE-AREA" digest="sha256:fb92ab357fb4b145cdc7655ac757f8100e1490424d08d1b77710c10f7815a3c6" -->

<!-- pdac:cite id="BR-AREAS-NAME-REQUIRED" digest="sha256:ef9473c4431f6692628ff6a334fdac8aaa63954aa59a63887ca33923d4c2ea6c" -->

<!-- pdac:cite id="BR-AREAS-OWNERSHIP-OPTIONAL" digest="sha256:b78ea5bf745d01ddf6df583c6fe2602466b54bbc9932ae19c7da2f5ff408ba47" -->

<!-- pdac:cite id="BR-AREAS-ONE-HOUSEHOLD" digest="sha256:91515f7d6b7fd63353017cdcb60be50936b5a7ab7ca31ad50a12ebfac3259808" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:e7196eaf5a47d7e4d00002fc29785bc463758d4fa47e08bcb279b9350d88a1f0" -->

<!-- pdac:cite id="TERM-AREA" digest="sha256:976ac0460848d3395277a844f2beb4d62e6a06214d77e3f34d399b9a3feaf74b" -->

<!-- pdac:cite id="SB-AREAS-CREATE-AREA" digest="sha256:1ed9b3e387eb749c13fc3f757108348ecfe01c9b140b8d1abd3b0c3f11cf2e56" -->

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

<!-- pdac:cite id="FR-AREAS-ASSIGN-OWNER" digest="sha256:4a6ecb340845e141e9c8e3205b761a8b69fcc0b4d45d6d00c46ad122e2798fd7" -->

<!-- pdac:cite id="UC-AREAS-ASSIGN-OWNER" digest="sha256:bcf0319631469de361b3c6f5092def082b6325947f0699a52b49d47460e0c529" -->

<!-- pdac:cite id="BR-AREAS-SINGLE-OWNER" digest="sha256:1238c8a78a73277bbef49a57570724b10b09670351a71001c3b9af9415e10c7c" -->

<!-- pdac:cite id="BR-AREAS-SAME-HOUSEHOLD-PEOPLE" digest="sha256:41928a2f064c528565e46acc98cf7ae03be0d0faf185e958e0917f16ed6f51e7" -->

<!-- pdac:cite id="BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE" digest="sha256:855d4fc5b699713e713de560e1317a4ce4851f0c9d54483a29b937c930feff33" -->

<!-- pdac:cite id="BR-AREAS-OWNER-NOT-SUPPORT" digest="sha256:dfc3f03fd053bfa38970a52ae3a604c37259cf2682c738810717995f710ddea4" -->

<!-- pdac:cite id="BR-AREAS-NO-INACTIVE-ASSIGNEE" digest="sha256:400d3d6076c12f5a4c3c1c9f56b7eb210f5fbac56534ef4019ec4a789aeb2775" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:e7196eaf5a47d7e4d00002fc29785bc463758d4fa47e08bcb279b9350d88a1f0" -->

<!-- pdac:cite id="TERM-AREA-OWNER" digest="sha256:a99a47f784d89b722cff7d894eb729fec19510330334e760805365920d8eff3a" -->

<!-- pdac:cite id="SB-AREAS-ASSIGN-FIRST-OWNER" digest="sha256:52ffbb43527539b2a78e75533c975bb579b5824bf1a8c713dc4c65aef8de1fd4" -->

<!-- pdac:cite id="SB-AREAS-REPLACE-OWNER" digest="sha256:43414f3b7af5b7b160bb2af8df55de628a395488e87e8c9b79356d94b6f3367c" -->

<!-- pdac:cite id="SB-AREAS-REJECT-OUTSIDE-OWNER" digest="sha256:940583d7d8d97fed26e4467dc3d25901000997b1ecbbe672caf6bbe2d5be7089" -->

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

<!-- pdac:cite id="FR-AREAS-ADD-SUPPORT" digest="sha256:c815cd85342fbc7af1629a74f742fa05a8266c0cb06183a3b64d8fa06b72b451" -->

<!-- pdac:cite id="UC-AREAS-ADD-SUPPORT" digest="sha256:33c7ceb949c5fb0ec239143d3f78f406245045e32953d10100025279c23f6d8a" -->

<!-- pdac:cite id="BR-AREAS-UNIQUE-SUPPORT" digest="sha256:92701de5b46bb57746247a6b391e40a6f2547a9f5d17186e669181d4962439d8" -->

<!-- pdac:cite id="BR-AREAS-SAME-HOUSEHOLD-PEOPLE" digest="sha256:41928a2f064c528565e46acc98cf7ae03be0d0faf185e958e0917f16ed6f51e7" -->

<!-- pdac:cite id="BR-AREAS-NO-INACTIVE-ASSIGNEE" digest="sha256:400d3d6076c12f5a4c3c1c9f56b7eb210f5fbac56534ef4019ec4a789aeb2775" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:e7196eaf5a47d7e4d00002fc29785bc463758d4fa47e08bcb279b9350d88a1f0" -->

<!-- pdac:cite id="TERM-AREA-SUPPORT" digest="sha256:dc9c68154fabba1f6b47fef6524b68377e39dadb93316f7dbe46d8bb281a6983" -->

<!-- pdac:cite id="SB-AREAS-ADD-SUPPORT" digest="sha256:61f46039a64ff4064a05ce75cfd4c8b52a29fd6bc050d1808daed584787f7e7a" -->

<!-- pdac:cite id="SB-AREAS-REJECT-DUPLICATE-SUPPORT" digest="sha256:a14909442ebad1b51039d22752a934e98b516f382299dd10bfb13521e277fb74" -->

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

<!-- pdac:cite id="FR-AREAS-REMOVE-SUPPORT" digest="sha256:39d8fb5aba06ed681f686f76a32aedb6bbc0163cb5b84a78192d81a528eabf72" -->

<!-- pdac:cite id="UC-AREAS-REMOVE-SUPPORT" digest="sha256:72739ad9f6fa2c61e4781da30ce179fec4aaad77ca67d2c0b91c1cf2d96f96d4" -->

<!-- pdac:cite id="BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE" digest="sha256:855d4fc5b699713e713de560e1317a4ce4851f0c9d54483a29b937c930feff33" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:e7196eaf5a47d7e4d00002fc29785bc463758d4fa47e08bcb279b9350d88a1f0" -->

<!-- pdac:cite id="TERM-AREA-SUPPORT" digest="sha256:dc9c68154fabba1f6b47fef6524b68377e39dadb93316f7dbe46d8bb281a6983" -->

<!-- pdac:cite id="SB-AREAS-REMOVE-SUPPORT" digest="sha256:b23a2b4e84cef6bbf94eb381d822cb6feb054f1486a8e5af11db1649577e6ec6" -->

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

<!-- pdac:cite id="FR-AREAS-TRANSFER-OWNERSHIP" digest="sha256:b80ef6f8810d8ebcb96ebcbd7ee12829e29aaba8e34d3e191d6f97237e2f4eb8" -->

<!-- pdac:cite id="UC-AREAS-ASSIGN-OWNER" digest="sha256:bcf0319631469de361b3c6f5092def082b6325947f0699a52b49d47460e0c529" -->

<!-- pdac:cite id="BR-AREAS-EXPLICIT-OWNERSHIP-CHANGE" digest="sha256:855d4fc5b699713e713de560e1317a4ce4851f0c9d54483a29b937c930feff33" -->

<!-- pdac:cite id="BR-AREAS-SAME-HOUSEHOLD-PEOPLE" digest="sha256:41928a2f064c528565e46acc98cf7ae03be0d0faf185e958e0917f16ed6f51e7" -->

<!-- pdac:cite id="BR-AREAS-SINGLE-OWNER" digest="sha256:1238c8a78a73277bbef49a57570724b10b09670351a71001c3b9af9415e10c7c" -->

<!-- pdac:cite id="TERM-RESPONSIBILITY-TRANSFER" digest="sha256:a467abc3510f94575ce11c84aa964e4e9d2ffdae453c3d4f27562e9fbcbdaced" -->

<!-- pdac:cite id="SB-AREAS-TRANSFER-OWNERSHIP" digest="sha256:8fe5971e8097d994eab6b45f46c6bab7c821be58f9baf16b331cf86a6f57a1e3" -->

<!-- pdac:cite id="SB-AREAS-REJECT-OUTSIDE-TRANSFER" digest="sha256:b58d0039455df440e6a879678150ef48af64d47a466eab3eb2b1a9db8c266768" -->

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

<!-- pdac:cite id="FR-AREAS-RENAME-AREA" digest="sha256:3bb1d9d4678b4b0f3c1ec26e23d185ae152dbd3841505a70a5e83775c425e3b6" -->

<!-- pdac:cite id="UC-AREAS-RENAME-AREA" digest="sha256:1df8637116f2bd1b1e931bd3522e67be9daf866d0878b55bde7c695bc401b8a5" -->

<!-- pdac:cite id="BR-AREAS-NAME-REQUIRED" digest="sha256:ef9473c4431f6692628ff6a334fdac8aaa63954aa59a63887ca33923d4c2ea6c" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:e7196eaf5a47d7e4d00002fc29785bc463758d4fa47e08bcb279b9350d88a1f0" -->

<!-- pdac:cite id="SB-AREAS-RENAME-AREA" digest="sha256:de410afd4f9fa661e17cbf07e07416de1aaa7e57b00d413816cf2ac88ebc82c6" -->

#### Scenario: Household renames an Area

- GIVEN an Area exists
- WHEN the household provides a new name for the Area
- THEN the Area is updated with the new name

---

### Requirement: Area Archiving

A household SHALL be able to archive an Area.

Archived Areas are retained and visible when filtered for, but are distinguished from active Areas.

<!-- pdac:cite id="FR-AREAS-ARCHIVE-AREA" digest="sha256:61858840e7a767e9780719fccaa49c2bf3d5fb5627b1832dfd717fe7eeb97536" -->

<!-- pdac:cite id="UC-AREAS-ARCHIVE-AREA" digest="sha256:067155f11be19859765494e77b687e08cb42b305b0ce61554d8e80221e783aa2" -->

<!-- pdac:cite id="BR-AREAS-ANY-MEMBER-MANAGES" digest="sha256:e7196eaf5a47d7e4d00002fc29785bc463758d4fa47e08bcb279b9350d88a1f0" -->

<!-- pdac:cite id="BR-TASKS-ARCHIVED-AREA-CLEARED" digest="sha256:3334e78310889e51497c45b324544bef1b473b448e751656b8f5ddcc1ff2a43d" -->

<!-- pdac:cite id="SB-AREAS-ARCHIVE-AREA" digest="sha256:b9239fe4023a7465e4f12bbdc71bc3a603959ad7827f9f305895bc25b0b485da" -->

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

<!-- pdac:cite id="FR-AREAS-OWNERSHIP-VISIBILITY" digest="sha256:acf5743c30edd22cc0badaaf5bc4d6998fb46c2e71dc2dc84452b7557a4b14d8" -->

<!-- pdac:cite id="UC-AREAS-REVIEW-OWNERSHIP" digest="sha256:bf641773cb1723b92c2eb3e20bfebb013ee1be7d3722c06df94cc97e56cc69a0" -->

<!-- pdac:cite id="TERM-OWNERSHIP-GAP" digest="sha256:6315a8195c46d7e4cae41ff65fcaf5a4a777c8d964661988b850cdc6102c4434" -->

<!-- pdac:cite id="JRN-AREAS-CLOSE-OWNERSHIP-GAPS" digest="sha256:205e4a10523c547486d210c134dc7a04698ed9425014196fca1a28e98e287004" -->

<!-- pdac:cite id="QR-AREAS-GAPS-AT-A-GLANCE" digest="sha256:f1a55dc43455210a0ed6dd85f11f80d67e68d984acd0d84e65e8f392fce5829f" -->

<!-- pdac:cite id="SB-AREAS-UNOWNED-FIRST" digest="sha256:47b6a5b5b96fe924545ba8d7fba1408ed5eaff98d3404ddb129ff5d09dd29b41" -->

<!-- pdac:cite id="SB-AREAS-GAP-INDICATOR" digest="sha256:3ddabbf8e8bd488cc243d27a98468f3b9bf74559e1f0092ad6d2b8848b4e6a6f" -->

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

<!-- pdac:cite id="FR-AREAS-CROSS-CONTEXT-REFERENCE" digest="sha256:79a713f51c250ee739e56168e25abf38566aa204af08f745fc6136c511c7d1a1" -->

<!-- pdac:cite id="BR-AREAS-OWNERSHIP-EXCLUSIVE" digest="sha256:0139a48e2daf32671e19923ef4917d8ab38585d4a224bfdafed1735dcb5c7eb0" -->

<!-- pdac:cite id="BR-TASKS-AREA-CATEGORISES-ONLY" digest="sha256:1c63c0149739660ca8a8626e6f2eb9260cab229cdb5bb667aebefb36c89e314e" -->

<!-- pdac:cite id="BC-RESPONSIBILITIES" digest="sha256:031c9bd3da1ee33bb97c7f5b66f5d37827db17703653d1b00013680d61c55e58" -->

<!-- pdac:cite id="SB-AREAS-TASK-REFERENCES-AREA" digest="sha256:d057b816b76bc1de4c4dec91f5ab82f757cdaf742aaa8548dbfe051758cf3c99" -->

#### Scenario: A task references an Area

- GIVEN a task is created with a reference to an Area
- THEN the Area context is recorded on the task as a categorization reference
- AND the Area's ownership assignments are not affected

---

## Notes

### Terminology convergence

"Area" (product surface) and "Responsibility Domain" (domain model) refer to the same concept. All repository sources are consistent on this mapping. "Owner" maps to Primary Owner; "Support" maps to Secondary Owner. This spec uses Area, Owner, and Support when describing product-facing behavior, and notes the domain terms where relevant.

`docs/04_contexts/areas.md` does not exist in this repository. The context document for this domain lives at `docs/04_contexts/responsibilities.md`. The Areas label is used by the product surface (`00_product/surfaces/areas.md`) and the navigation entry point.

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

- `docs/04_contexts/responsibilities.md`
- `00_product/surfaces/areas.md`
- `specs/features/responsibilities/create-responsibility-domain.md`
- `specs/features/responsibilities/assign-primary-owner.md`
- `specs/features/responsibilities/assign-secondary-owner.md`
- `specs/features/responsibilities/transfer-responsibility.md`
- `docs/03_domain/ubiquitous-language.md`
