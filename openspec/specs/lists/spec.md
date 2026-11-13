# Lists Specification

<!-- pdac-scope: cited -->

## Purpose

Lists are the household's persistent execution containers. A list answers: what should be remembered, bought, checked, prepared, or done next time?

A list is not a task board. A list item is not a task. Lists own capture and flexible execution across a spectrum: from a plain memory entry (name only) to a time-aware, importance-flagged item that projects into the Agenda surface.

Lists are reusable by design. Items are consumable within a list. The list persists across uses.

Lists are scoped to the household (family). They may be optionally associated with a household Area or a Plan as contextual anchors, but these links are informational — linking a list to a plan does not convert its items to scheduled work, and linking to an Area does not affect ownership or responsibility.

List state is shared in real time across household members — additions, toggles, and updates are visible to all without a manual refresh.

<!-- pdac:cite id="BC-LISTS" digest="sha256:7a770f21b03b8d0f769f46552bc0ef53fd885d5ce0ab41d081d831d2aa74ae4c" -->

<!-- pdac:cite id="TERM-LIST" digest="sha256:3972250cf543d9e8e6ea53b2472fb03ba2328475f47ab3202b4e867781b0fd48" -->

<!-- pdac:cite id="TERM-LIST-ITEM" digest="sha256:c6112cdc3c6f6573d978a595cec94145f47269189357e3e50257d39667ca56ee" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:3d1bef6b9be254d8683460f8e32fdc7e3dfe406720927975e5ec5dbc646f1faa" -->

<!-- pdac:cite id="CON-LISTS-ITEM-CAPABILITY-BOUNDARY" digest="sha256:f0b4e3bfe38b1249e7261996f8b21420e271b1c5ee0d349e7a42264f23bdfe9e" -->

<!-- pdac:cite id="BR-LISTS-HOUSEHOLD-SCOPED" digest="sha256:4b6ceded7b878e265d641f2ebf7a89d99c1d32e15cf0c25231c8591d1d76f199" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:c5d604a88b3d0a26c3005ee116c6145fd451a9fd4e2f1b56f4e48f0c5fdd7f16" -->

<!-- pdac:cite id="BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT" digest="sha256:2bdc7fe6d5bc3727361258ac8d5fd7e22f16d3056002e48b7eaa663a2ff1c150" -->

<!-- pdac:cite id="QR-LISTS-REAL-TIME-SHARING" digest="sha256:f6395989e6f67550ef00dca8128c89a18c06d7f918afd3c90d3300b5e2c64b9e" -->

<!-- pdac:cite id="JRN-LISTS-REUSE-A-LIST" digest="sha256:b6207a0738ce655272ca0575c950bc6a5b5fc830ca0e9dc116365b2e9eff73dc" -->

<!-- pdac-drift ids="BR-LISTS-PRIVATE-LIST, FR-LISTS-PRIVATE-LIST, BR-LISTS-HOUSEHOLD-SCOPED" summary="Purpose presents every list as household-scoped and shared with all members; the model lets a list be private to one person (Q-0027)" -->

---

## Requirements

### Requirement: List Creation

A household SHALL be able to create a list with a name as the only required input.

Optional associations at creation: an Area (contextual memory for that Area), a Plan (list is used in context of that plan), and a kind (system-level classification). All optional fields may also be set or changed after creation.

A list is initialized with an empty item collection.

<!-- pdac:cite id="FR-LISTS-CREATE-LIST" digest="sha256:25c9cc803d305300a243539bd99da341cbda24a290f61af4894dbd02f91b0e6d" -->

<!-- pdac:cite id="SB-LISTS-CREATE-NAME-ONLY" digest="sha256:67ca544ab5591dbd2d620615cf09d68da36f4a8da202529356e9f571083ec2ed" -->

<!-- pdac:cite id="SB-LISTS-CREATE-LINKED-TO-PLAN" digest="sha256:4e14bcad3e1cb911e53dc7975241e0ceab0a6953a1ea11fbfe67b921a2e51450" -->

<!-- pdac:cite id="UC-LISTS-CREATE-LIST" digest="sha256:82df9acf755674420aae2f412b71663853188987c855c12931e78c38a4db8f4d" -->

<!-- pdac:cite id="BR-LISTS-LIST-NAME-REQUIRED" digest="sha256:95049208f08bebfb977ee6a6bad5f34e2ceef9fbd21bff7aadfc78cdf7fd6d29" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:c5d604a88b3d0a26c3005ee116c6145fd451a9fd4e2f1b56f4e48f0c5fdd7f16" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:3d1bef6b9be254d8683460f8e32fdc7e3dfe406720927975e5ec5dbc646f1faa" -->

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:c49275adaa16e46f1d1613af96372c572b5562e38912ce8db7693be665d6e4ec" -->

<!-- pdac:cite id="TERM-LIST" digest="sha256:3972250cf543d9e8e6ea53b2472fb03ba2328475f47ab3202b4e867781b0fd48" -->

<!-- pdac:cite id="TERM-LIST-KIND" digest="sha256:2f2700e38b8910125fd9b57d88c64cc3dac9bc863978f52bd2924010052b483e" -->

#### Scenario: Household creates a list with a name only

- GIVEN a family exists
- WHEN the household creates a list with a valid name and no other fields
- THEN the list is created and available for use
- AND it is initialized with no items

#### Scenario: Household creates a list linked to a plan

- GIVEN a family exists
- WHEN the household creates a list with a name and a linked plan
- THEN the list is created and associated with the plan
- AND the list retains list semantics — items are not converted to tasks or scheduled entries

---

### Requirement: List Retrieval

A household SHALL be able to retrieve a summary of all active lists for the family.

Each summary includes the list name and unchecked item count. Archived lists are excluded from the default result.

<!-- pdac:cite id="FR-LISTS-BROWSE-LISTS" digest="sha256:5b9e3226a32138d68f98657ee01c2530bcbf39f097e41b55e7a18c8b6f8efc26" -->

<!-- pdac:cite id="SB-LISTS-BROWSE-ACTIVE" digest="sha256:9be2ef19668384adc1d060e49bbeab9ec2542d1e91e01098f0107414f20389f6" -->

<!-- pdac:cite id="SB-LISTS-BROWSE-EXCLUDES-ARCHIVED" digest="sha256:4a56bb8052928788ee4ee66723891bfccaa1dc1eb46bc65f1aefdec4d47c9273" -->

<!-- pdac:cite id="UC-LISTS-BROWSE-LISTS" digest="sha256:3edc8eb98802d84347ef8ae624a29dc866c5dcf0939db22a0067288b42f87f7b" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:3283e585a3a3a503e6af9b70f17f75c99dcc79f5e3a0b7ad58cbaa29bf5ed860" -->

#### Scenario: Household views the list switcher

- GIVEN a family has several active lists
- WHEN the household requests the family's lists
- THEN all active lists are returned with their names and unchecked item counts
- AND archived lists are not included

#### Scenario: Archived list is excluded

- GIVEN a list has been archived
- WHEN the household requests the family's active lists
- THEN the archived list does not appear in the results

---

### Requirement: List Detail

A household SHALL be able to retrieve the full content of a list, including all items.

Items are returned in two groups: unchecked items first, then checked items. Within each group, items are ordered by their stable display order. All items are included regardless of checked state.

<!-- pdac:cite id="FR-LISTS-OPEN-LIST" digest="sha256:7b22d78453a3e7e8c72f95e9391ab646c664da7e5c9afe7b39a3948efa9e6bfd" -->

<!-- pdac:cite id="SB-LISTS-OPEN-UNCHECKED-FIRST" digest="sha256:7bd0a97f8e052ca3c996fde32163ba3062bf5079a9189747164b7cfabae33def" -->

<!-- pdac:cite id="UC-LISTS-OPEN-LIST" digest="sha256:8e1a65110f288baa5f7fb827197590037cb9e7255c9cb1d61ac0c85d656cfa10" -->

<!-- pdac:cite id="TERM-CHECKED-ITEM" digest="sha256:92b2028e2e875bd9b994751aa25de16c9928b4204734ca2cfe7a77e1ff1bd94b" -->

#### Scenario: Household opens a list

- GIVEN a list exists with a mix of checked and unchecked items
- WHEN the household retrieves the list detail
- THEN all items are returned
- AND unchecked items appear before checked items

---

### Requirement: List Update

A household SHALL be able to update a list's name and optional associations.

Updatable fields: name, Area association, linked Plan, kind. At least one field must be provided. Fields not included in the request are unchanged. Area association and Plan linkage may be cleared explicitly.

<!-- pdac:cite id="FR-LISTS-UPDATE-LIST" digest="sha256:a5698e5ecdb6992e2c43258f224916174fd099cca0c59f6ba559391ca3d9901f" -->

<!-- pdac:cite id="SB-LISTS-RENAME" digest="sha256:7d6f32d6299e0b4fa84c2822a51deb17abc1a6bdf08eb435c67bcaced04c0575" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-PLAN-LINK" digest="sha256:6aad6d7f2b8338dd0fcadda2efe32d87ce19b9e38365b24e13c8b5d7945c74d0" -->

<!-- pdac:cite id="UC-LISTS-UPDATE-LIST" digest="sha256:beeba258dbe3f6ba50f32647f6b064962b1f00a98e110f816e1f4f75f702b52a" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:c5d604a88b3d0a26c3005ee116c6145fd451a9fd4e2f1b56f4e48f0c5fdd7f16" -->

<!-- pdac:cite id="BR-LISTS-LIST-NAME-REQUIRED" digest="sha256:95049208f08bebfb977ee6a6bad5f34e2ceef9fbd21bff7aadfc78cdf7fd6d29" -->

<!-- pdac:cite id="TERM-LIST-KIND" digest="sha256:2f2700e38b8910125fd9b57d88c64cc3dac9bc863978f52bd2924010052b483e" -->

#### Scenario: Household renames a list

- GIVEN a list exists
- WHEN the household provides a new name
- THEN the list is updated with the new name
- AND no other fields are affected

#### Scenario: Household removes a plan link

- GIVEN a list is linked to a plan
- WHEN the household updates the list and explicitly clears the plan link
- THEN the list is no longer linked to the plan
- AND the list's items are unaffected

---

### Requirement: List Archive

A household SHALL be able to archive a list that is no longer in active use.

Archiving transitions the list out of the active collection. All data is preserved — items retain their names, quantities, notes, and checked states. No items are removed or modified. The archive operation has no effect on linked Areas, Plans, or any other context.

A list that is already archived cannot be archived again.

<!-- pdac:cite id="FR-LISTS-ARCHIVE-LIST" digest="sha256:050b27be17e6b043e91241e56988e8099bc62e18ff07fe964842e197bfe11477" -->

<!-- pdac:cite id="SB-LISTS-ARCHIVE" digest="sha256:6c0a6878c819ada87235cd49ddfa67d8490441f05dc2a563a3f484f02bc04bf0" -->

<!-- pdac:cite id="SB-LISTS-ARCHIVE-TWICE-REJECTED" digest="sha256:a968324eb8311ee8ac685c0f8514ea6d8e1337253f27ca8c3fb3180d453f9364" -->

<!-- pdac:cite id="UC-LISTS-ARCHIVE-LIST" digest="sha256:1b1904e7a6583bce2e708950e1b87060cdf5c7ac0ae7432036226211825a0cde" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVE-STATE" digest="sha256:eec8e50fbefbbf5d6ae56d3650200f956e319ff65ae7201b86526807a8e1c93e" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVED-READ-ONLY" digest="sha256:2efea969d9cadd61803b9e2fcf708cb7ace6bf72d3daa5a2873f860ded04193b" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:3283e585a3a3a503e6af9b70f17f75c99dcc79f5e3a0b7ad58cbaa29bf5ed860" -->

<!-- pdac-drift ids="BR-LISTS-ARCHIVED-READ-ONLY, FR-LISTS-ARCHIVE-LIST, TERM-ARCHIVED-LIST" summary="Spec does not make an archived list read-only and its item toggle, update, importance and timing requirements apply to any item; the model rejects item changes on an archived list until it is restored (Q-0031)" -->

#### Scenario: Household archives a list

- GIVEN an active list exists
- WHEN the household archives it
- THEN the list is no longer returned in the default active list query
- AND all items remain intact

#### Scenario: Household attempts to archive an already-archived list

- GIVEN a list is already archived
- WHEN the household attempts to archive it again
- THEN the operation is rejected

---

### Requirement: List Restore

A household SHALL be able to restore an archived list to active use.

Restore is the symmetric inverse of archive. No item data is modified. The restored list behaves identically to any other active list. A list that is not archived cannot be restored.

<!-- pdac:cite id="FR-LISTS-RESTORE-LIST" digest="sha256:0deba81893a356379ca3ba12744d9856c42b1a53fc7371cae53853b665bb00b3" -->

<!-- pdac:cite id="SB-LISTS-RESTORE" digest="sha256:aeecdcaa3868472dad35760476ef751faf801c90db5841359f38e26ef4b57e0b" -->

<!-- pdac:cite id="UC-LISTS-RESTORE-LIST" digest="sha256:8d6749745dfdea956d173ff87481388095f41038b3308451ff609703da340667" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVE-STATE" digest="sha256:eec8e50fbefbbf5d6ae56d3650200f956e319ff65ae7201b86526807a8e1c93e" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:3283e585a3a3a503e6af9b70f17f75c99dcc79f5e3a0b7ad58cbaa29bf5ed860" -->

#### Scenario: Household restores an archived list

- GIVEN an archived list exists
- WHEN the household restores it
- THEN the list appears again in the default active list query
- AND all items are in the same state as when the list was archived

---

### Requirement: Item Addition

A household SHALL be able to add a new item to an active list with only a name.

A new item is always created in an unchecked state. Optional fields at creation: quantity and note. Items are appended in stable order — each new item receives the next position. Importance and temporal fields are not set at item creation; they are applied through dedicated operations after creation. An assistant may stage an addition as an Inbox proposal, but Lists changes only after an authorized member approves it.

<!-- pdac:cite id="FR-INBOX-INTERPRET" digest="sha256:8f2efe33de44deac70caac674756cd003907c878b4bdfb69128b6f5515bb6f4f" -->
<!-- pdac:cite id="BR-INBOX-REVIEW-AND-EXECUTION" digest="sha256:92d34d343ebc239e46c834106ac81dc978c2f014b9ae1e5574786cc82da7821e" -->

<!-- pdac:cite id="FR-LISTS-ADD-ITEM" digest="sha256:31ca6047288c0e531133f611eda3ba2b8c142fc5d991cc0015103929474a2ff3" -->

<!-- pdac:cite id="SB-LISTS-ADD-ITEM" digest="sha256:ab8758ad6cc21c132a9d60afca8b0a664b3b2c3739d1f3b86978db3079d65e7d" -->

<!-- pdac:cite id="SB-LISTS-SEQUENTIAL-CAPTURE" digest="sha256:f71db1393adbc83b54644275fa8d6f276a304a59ac74ea138432bd12ca5bfa44" -->

<!-- pdac:cite id="UC-LISTS-ADD-ITEM" digest="sha256:225a14936af4adf944fc467e7ade4f6dd560a20d6937dac48be51c3afd26663a" -->

<!-- pdac:cite id="BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED" digest="sha256:48d80f3f2f4e8dc648d33b357c5e03eea4e592d72ec921beb4d04c630e9347d8" -->

<!-- pdac:cite id="BR-LISTS-ITEM-NAME-REQUIRED" digest="sha256:5293bba721343a22f68c8e4c51ca3aeffe8c22bbb7bc27b7362d67738cc0ac77" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVED-READ-ONLY" digest="sha256:2efea969d9cadd61803b9e2fcf708cb7ace6bf72d3daa5a2873f860ded04193b" -->

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:c49275adaa16e46f1d1613af96372c572b5562e38912ce8db7693be665d6e4ec" -->

<!-- pdac:cite id="TERM-LIST-ITEM" digest="sha256:c6112cdc3c6f6573d978a595cec94145f47269189357e3e50257d39667ca56ee" -->

#### Scenario: Household adds an item to a list

- GIVEN a list exists
- WHEN the household adds an item with a name
- THEN the item is created unchecked at the end of the list
- AND it is immediately available in the list

#### Scenario: Sequential item capture

- GIVEN the household adds multiple items in sequence
- THEN each item is appended in the order it was added
- AND no modal or interruption is required between items

---

### Requirement: Item Update

A household SHALL be able to update the base fields of a list item: name, quantity, and note.

At least one of these fields must be provided. Fields not included are left unchanged. Quantity may be cleared explicitly. This operation does not affect checked state, importance, temporal fields, or item order. An assistant may stage these supported fields as an Inbox proposal, but Lists changes only after an authorized member approves it.

<!-- pdac:cite id="FR-LISTS-UPDATE-ITEM" digest="sha256:ac9c993fe7be9604213732cfbb89cd07b9cd1ef6b234946e7889c3da28b98005" -->

<!-- pdac:cite id="SB-LISTS-RENAME-ITEM" digest="sha256:bfddfc7ee76362815bf8e464fda2c8da2ada6765e985599e30292a223afc057e" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-QUANTITY" digest="sha256:44bb4a4730448dfe9abf39cdd635f4182ad70fd28c3c9117a1ec0249256c4a78" -->

<!-- pdac:cite id="UC-LISTS-UPDATE-ITEM" digest="sha256:da03e156bcb1280aeb5b60d118c85ad437d579fb7b4e5d14331e229d7b51cdc9" -->

<!-- pdac:cite id="BR-LISTS-ITEM-NAME-REQUIRED" digest="sha256:5293bba721343a22f68c8e4c51ca3aeffe8c22bbb7bc27b7362d67738cc0ac77" -->

#### Scenario: Household updates an item's name

- GIVEN a list item exists
- WHEN the household provides a new name
- THEN the item name is updated
- AND the item's checked state, importance, and temporal fields are unchanged

#### Scenario: Household clears an item's quantity

- GIVEN a list item has a quantity set
- WHEN the household explicitly clears the quantity
- THEN the item no longer has a quantity
- AND the item name and other fields are unchanged

---

### Requirement: Item Toggle

A household SHALL be able to toggle the checked state of any item in a list.

Toggle is binary: unchecked → checked (item is handled for this use), or checked → unchecked (item is relevant again for the next use). Toggling an item does not remove it from the list, does not affect its importance or temporal fields, and has no effects outside the list.

A checked item with temporal fields continues to project into the Agenda surface in a de-emphasized state. Toggle does not clear temporal fields.

<!-- pdac:cite id="FR-LISTS-TOGGLE-ITEM" digest="sha256:c158a8a3a674aa12d9b60772dba45a3ff914a75d8bb088940a1f9de5f734e119" -->

<!-- pdac:cite id="SB-LISTS-CHECK-ITEM" digest="sha256:2a1f5b606cbc228a292710d8aa511ed9f66164aa4d83ec3b3a4d74f91479ebae" -->

<!-- pdac:cite id="SB-LISTS-UNCHECK-ITEM" digest="sha256:eaea2752d02c18887c571fd0cf8d6c3af699dff58c4ab6804b0965fafe310f3b" -->

<!-- pdac:cite id="SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA" digest="sha256:b39da8440e6bb567d87d951698c935b3d9dff0f5bc564bd8f9c27a362cf0fd26" -->

<!-- pdac:cite id="UC-LISTS-TOGGLE-ITEM" digest="sha256:591b03f8b2639723dea6a3788c966a07d62a753f8b135195463a686386cb656b" -->

<!-- pdac:cite id="BR-LISTS-TOGGLE-PRESERVES-ITEM" digest="sha256:7825a92a0bf1847c2d536815a9df1b089d8b5005f3d534cb675cb9c3d86dde59" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:043eef067ee7db97dd23b7dc3dc2e7c9b7c52b4c9f5aefc6e3d2066abd179350" -->

<!-- pdac:cite id="TERM-CHECKED-ITEM" digest="sha256:92b2028e2e875bd9b994751aa25de16c9928b4204734ca2cfe7a77e1ff1bd94b" -->

#### Scenario: Household checks an item

- GIVEN an unchecked item exists in a list
- WHEN the household toggles it
- THEN the item becomes checked
- AND it remains in the list

#### Scenario: Household unchecks an item

- GIVEN a checked item exists in a list
- WHEN the household toggles it
- THEN the item becomes unchecked
- AND it is treated as relevant again

#### Scenario: Checked item with a due date remains in Agenda

- GIVEN an item has a due date and is checked
- WHEN the household views the Agenda for that date
- THEN the item still appears in a de-emphasized state
- AND it is not removed from projection until its temporal fields are cleared

---

### Requirement: Item Removal

A household SHALL be able to permanently remove an item from a list.

Removal is immediate and irreversible. The item is deleted from the list's ordered collection. If the removed item had temporal fields, it is removed from Agenda projection. Removal has no effects outside the list.

<!-- pdac:cite id="FR-LISTS-REMOVE-ITEM" digest="sha256:b08b2488166905a607c953f20daa9e6312a3d05e0f9b271b0e155d8d31e52ba4" -->

<!-- pdac:cite id="SB-LISTS-REMOVE-ITEM" digest="sha256:c3f9ff2377c28efb75b99aca195cc104e1df62f2e6c8d8c8a18a158023bb7ba0" -->

<!-- pdac:cite id="SB-LISTS-REMOVE-DATED-ITEM" digest="sha256:4442edd8e796c9f1856ce5e901174968915b83c88ff80249c50fdcb1b2126c3e" -->

<!-- pdac:cite id="UC-LISTS-REMOVE-ITEM" digest="sha256:2a51e7adc13cc1faedcd697f14bad87a91fbc9b9ec7eaf3056c0b7d3769c8a33" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:043eef067ee7db97dd23b7dc3dc2e7c9b7c52b4c9f5aefc6e3d2066abd179350" -->

#### Scenario: Household removes an item

- GIVEN a list item exists
- WHEN the household removes it
- THEN the item is no longer present in the list
- AND the remaining items are unchanged

#### Scenario: Removing an item with temporal fields clears its Agenda projection

- GIVEN a list item with a due date is projecting into Agenda
- WHEN the household removes the item
- THEN the item no longer appears in Agenda

---

### Requirement: Item Reorder

A household SHALL be able to set the display order of items within a list.

The operation takes the complete intended order as a full replacement. The provided list must contain exactly the current set of items — no additions, no omissions. Order carries no semantic meaning (no priority, urgency, or importance). Checked and unchecked items are part of the same ordered sequence; display grouping is a surface concern.

<!-- pdac:cite id="FR-LISTS-REORDER-ITEMS" digest="sha256:bfc6bee0b15b5393466a520f857b6dbb5d7e49b95fac7deff9604eece2caa818" -->

<!-- pdac:cite id="SB-LISTS-REORDER" digest="sha256:dadb735f9bda046cb922cb422082045ba3298ad2c21d43c62c0a9e01f3da2f8e" -->

<!-- pdac:cite id="SB-LISTS-REORDER-MISMATCH-REJECTED" digest="sha256:9d3b7d2a873d66436bf961c717c13edbfe357bd67002324f826add5466a6ca6c" -->

<!-- pdac:cite id="UC-LISTS-REORDER-ITEMS" digest="sha256:c2f711de4be452a677c289178af777c6ef0ad86bcf2470b02f6302c3beede29b" -->

<!-- pdac:cite id="BR-LISTS-REORDER-FULL-SET" digest="sha256:864f8ce571b1a75902305f975acc65a3e15e21fa3c824926f709edee23a9406d" -->

#### Scenario: Household reorders items

- GIVEN a list with several items
- WHEN the household provides a complete new order for those items
- THEN each item is assigned its new position
- AND no item field other than order is modified

#### Scenario: Reorder with mismatched item set is rejected

- GIVEN a list with three items
- WHEN the household provides a reorder containing only two of the three items
- THEN the operation is rejected
- AND the current item order is preserved

---

### Requirement: Item Importance

A household SHALL be able to mark or unmark any list item as important.

Importance is a binary flag (starred / not-starred). It is not a score or ranking. Setting importance to a value that is already set is a no-op. Importance does not affect temporal fields, checked state, or Agenda projection eligibility.

<!-- pdac:cite id="FR-LISTS-ITEM-IMPORTANCE" digest="sha256:ce302425272a44f0fe0719f0589804bcc94e294a07d4e4b486ad7b19f90ad394" -->

<!-- pdac:cite id="SB-LISTS-STAR-ITEM" digest="sha256:cb288cc57fc0c9a0266e6c33f2efecf2a5d4c904830a3888a14bf67e4dda9203" -->

<!-- pdac:cite id="SB-LISTS-STAR-IDEMPOTENT" digest="sha256:469f1deb26c3db416a7f6b715bdb7cb5bb9ee4e37cd50f57f79b7b6a6abe606e" -->

<!-- pdac:cite id="UC-LISTS-SET-ITEM-IMPORTANCE" digest="sha256:e9ee3917dd20c6c3b2489bb69338cd8b42597b8011f48d62341f7cd4afd1c8e7" -->

<!-- pdac:cite id="BR-LISTS-IMPORTANCE-BINARY" digest="sha256:b779b624287684ff2c848bbf1152cd268f2d4d8e0849e3fe391010817143c4d2" -->

<!-- pdac:cite id="TERM-ITEM-IMPORTANCE" digest="sha256:e3bbf50c0811e62b653decd8f04508de22754a2f836fd939ffa7cd4fe1ac0de7" -->

#### Scenario: Household marks an item as important

- GIVEN a list item with no importance set
- WHEN the household sets importance to true
- THEN the item is marked as starred
- AND no other item fields are affected

#### Scenario: Setting importance is idempotent

- GIVEN a list item already marked as important
- WHEN the household sets importance to true again
- THEN the operation succeeds without error
- AND the item state is unchanged

---

### Requirement: Item Temporal Assignment

A household SHALL be able to assign temporal fields to a list item: due date, reminder, and/or repeat rule.

At least one temporal field must be provided per operation. Fields not included in the request are left unchanged. Setting any temporal field makes the item eligible for Agenda projection. Setting temporal fields does not convert the item to a task and does not create a calendar event.

When an item transitions from having no temporal fields to having at least one, this is treated as a distinct scheduling event. Subsequent updates to already-set temporal fields are treated as updates.

<!-- pdac:cite id="FR-LISTS-SET-ITEM-TIMING" digest="sha256:b3cbffce65b3e20f553f712ea2729544a94c6e4a9bc888619b60e7a58eb41c14" -->

<!-- pdac:cite id="SB-LISTS-SET-DUE-DATE" digest="sha256:9004b57d0b5c5577d6a1378a626ab2ae38e81e5e89c5c42fda4e65e2b52e404e" -->

<!-- pdac:cite id="SB-LISTS-SET-REMINDER-ONLY" digest="sha256:db40c73557269f3b216d94170715d769cbfc6bb2a1cfccc4679a65a496d825ab" -->

<!-- pdac:cite id="SB-LISTS-SET-REPEAT-ONLY" digest="sha256:8d1cb8d55715a6d35a4d46352756355e8b5382ed26bcdb4e4b1c9ddde9d9fd74" -->

<!-- pdac:cite id="UC-LISTS-SCHEDULE-ITEM" digest="sha256:675600256096facb5293fcbf96e179ba667cd76c400e91d3d7ddf8f1ac5dcff7" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:88fa4dda19ba3d6cd4e9e92b1d43d08fb0d96717d76e1428747adf70df3e55bb" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:3d1bef6b9be254d8683460f8e32fdc7e3dfe406720927975e5ec5dbc646f1faa" -->

<!-- pdac:cite id="TERM-ITEM-TEMPORAL-FIELDS" digest="sha256:15498c7a4246e94ae298569f73d3f6cfa6efdbc366631baae6fe03a5fc8b8578" -->

<!-- pdac:cite id="TERM-ITEM-REPEAT-RULE" digest="sha256:c17e5b8a9590f4c589c30a6ced4799cd5dc4c15de14f93839ab3529e13e952b4" -->

#### Scenario: Household sets a due date on an item

- GIVEN a list item with no temporal fields
- WHEN the household sets a due date
- THEN the item becomes eligible for Agenda projection on that date
- AND the item is not converted to a task

#### Scenario: Household sets a reminder without a due date

- GIVEN a list item with no temporal fields
- WHEN the household sets only a reminder (absolute date-time)
- THEN the item becomes eligible for Agenda projection at the reminder time
- AND no due date is required

#### Scenario: Household sets a repeat rule

- GIVEN a list item
- WHEN the household sets a repeat rule
- THEN the item becomes eligible for Agenda projection on each recurrence occurrence
- AND the repeat rule alone is sufficient for projection eligibility

---

### Requirement: Item Temporal Clearing

A household SHALL be able to remove all temporal fields from a list item in a single operation.

All three fields — due date, reminder, and repeat rule — are cleared atomically. After clearing, the item no longer projects into the Agenda surface. The item is not deleted and all other fields are unchanged.

If the item has no temporal fields, the operation is a no-op and succeeds silently.

<!-- pdac:cite id="FR-LISTS-CLEAR-ITEM-TIMING" digest="sha256:befbf601bbf68f6cda4e54ee4af1a8642208b44c15c1692005c7da958f2b3a50" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-TIMING" digest="sha256:fb816b34f5338e59dc16ac188f414b2e7020d1790f2a5d39ef1baf23a08c4a5f" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-TIMING-NOOP" digest="sha256:3caee20f8cb652bb7a29ac39235e753522cb9f066069114fb37a4830371fbd86" -->

<!-- pdac:cite id="UC-LISTS-CLEAR-ITEM-SCHEDULE" digest="sha256:05f0d58c57bd00c204408ac467a377cf46cb4cd0c45a8ac8c55b715000691046" -->

<!-- pdac:cite id="TERM-ITEM-TEMPORAL-FIELDS" digest="sha256:15498c7a4246e94ae298569f73d3f6cfa6efdbc366631baae6fe03a5fc8b8578" -->

#### Scenario: Household clears temporal fields from an item

- GIVEN a list item with a due date and a reminder
- WHEN the household clears the item's temporal fields
- THEN the item no longer appears in Agenda
- AND the item name, importance, checked state, and other fields are unchanged

#### Scenario: Clearing temporal fields on an item with none is safe

- GIVEN a list item with no temporal fields
- WHEN the household clears the temporal fields
- THEN the operation succeeds without error
- AND the item is unchanged

---

### Requirement: Agenda Projection

List items with any temporal field SHALL project into the Agenda surface as a distinct entry type.

An item projects when any of the following conditions is satisfied:
- its due date falls within the Agenda's requested date window
- its reminder datetime falls within the Agenda's requested date window
- its repeat rule produces an occurrence within the requested date window

All three conditions are independently sufficient. No temporal field requires another as a prerequisite.

Projected list items are read-only in Agenda. They carry a visible list-origin cue. Editing an item is done through the Lists surface only.

Checked items that meet a projection condition still appear in Agenda, de-emphasized. They are not removed from projection until their temporal fields are cleared.

<!-- pdac:cite id="FR-LISTS-AGENDA-PROJECTION" digest="sha256:b94162c9eb42af755596bd3448c69f727b83d6dd2a707631568d90ea10453d50" -->

<!-- pdac:cite id="SB-LISTS-DUE-TODAY-IN-AGENDA" digest="sha256:e6df1ad8bf5fd5a70a423032de6fbd1f91c568ab0251ff5ccf5ce4a23c60726e" -->

<!-- pdac:cite id="SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA" digest="sha256:b39da8440e6bb567d87d951698c935b3d9dff0f5bc564bd8f9c27a362cf0fd26" -->

<!-- pdac:cite id="SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA" digest="sha256:435869f319b3f053ea27df3bc2eb5b8d6f9dd5a0bf910fbe2201071d409a806b" -->

<!-- pdac:cite id="UC-LISTS-SEE-ITEMS-IN-AGENDA" digest="sha256:19f437a80c20b0934963cfd836845c4f7d45d3caa9e31b73984860dfd63cc900" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:043eef067ee7db97dd23b7dc3dc2e7c9b7c52b4c9f5aefc6e3d2066abd179350" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:a3c72a044c52c99808fa103720f30e8737d55fc1f7c9b6d7cc78e56c7539359c" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:88fa4dda19ba3d6cd4e9e92b1d43d08fb0d96717d76e1428747adf70df3e55bb" -->

<!-- pdac:cite id="TERM-PROJECTED-LIST-ITEM" digest="sha256:1d7dee71c8e9ef330968975a9a7b48e15b547f1b823286d462ab6b5ed059078f" -->

#### Scenario: Item with due date appears in Agenda on that date

- GIVEN a list item with a due date set to today
- WHEN the Agenda for today is loaded
- THEN the item appears as a projected list item with a list-origin cue
- AND it is visually distinct from tasks and plans

#### Scenario: Checked item continues to project

- GIVEN a list item with a due date that has been checked
- WHEN the Agenda for that date is loaded
- THEN the item still appears in Agenda
- AND it is de-emphasized

#### Scenario: Item is removed from Agenda after temporal fields are cleared

- GIVEN a list item that was projecting into Agenda
- WHEN the household clears all temporal fields
- THEN the item no longer appears in Agenda
- AND the item continues to exist in the list

---

## Notes

### Terminology: "Shared Lists" vs "Lists"

The context document (`docs/04_contexts/shared-lists.md`) and the item model document use "Shared Lists" and "SharedList" throughout. The product surface (`00_product/surfaces/lists.md`) and feature specs use "Lists". This spec uses "Lists" as the product-facing term. The transition is incomplete in the domain docs; "SharedList" remains the aggregate name in the domain model.

### Contradiction: `repeat` independence — Resolved

The `set-item-temporal` feature spec previously stated that `repeat` requires a `dueDate` to be present, contradicting the item model document which states that `repeat` is independently sufficient for Agenda projection. This contradiction is resolved by `docs/01_system/system-spec.md`, which explicitly states: "Repeat on a list item may be set independently of due date. Repeat is itself a temporal anchor sufficient for Agenda projection." The item model canonical position is confirmed. The `set-item-temporal` feature spec constraint was stale.

### `get-list-detail` item fields

The `get-list-detail` feature spec defines the per-item result shape as: name, quantity, note, checked, order — and explicitly excludes temporal and importance fields. This contradicts the item capability model, which makes these fields part of the item's full state. The item model document is marked as locking canonical state before implementation. The detail spec appears to predate it and should be updated to include importance and temporal fields in the item result.

### Partial temporal field clearing

`ClearSharedListItemTemporal` clears all three fields atomically. To clear only specific temporal fields individually, the `SetSharedListItemTemporal` operation is used by setting specific fields to null. This asymmetry is intentional per the source.

### Shopping list creation from Meal Planning

The context document notes that the Lists context receives shopping lists generated from Meal Planning as a `List` of kind `shopping`. The reception side behavior (how a shopping list lands in Lists) is owned by the Meal Planning spec, not this spec. The Lists context treats a shopping list as a regular list once created.

---

## Source References

- `docs/04_contexts/shared-lists.md`
- `docs/04_contexts/shared-lists-item-model.md`
- `00_product/surfaces/lists.md`
- `specs/features/lists/create-list.md`
- `specs/features/lists/update-list.md`
- `specs/features/lists/archive-list.md`
- `specs/features/lists/restore-list.md`
- `specs/features/lists/get-family-lists.md`
- `specs/features/lists/get-list-detail.md`
- `specs/features/lists/add-item-to-list.md`
- `specs/features/lists/update-list-item.md`
- `specs/features/lists/reorder-list-items.md`
- `specs/features/lists/toggle-list-item.md`
- `specs/features/lists/set-item-importance.md`
- `specs/features/lists/set-item-temporal.md`
- `specs/features/lists/clear-item-temporal.md`
