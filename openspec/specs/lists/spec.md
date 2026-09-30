# Lists Specification

<!-- pdac-scope: cited -->

## Purpose

Lists are the household's persistent execution containers. A list answers: what should be remembered, bought, checked, prepared, or done next time?

A list is not a task board. A list item is not a task. Lists own capture and flexible execution across a spectrum: from a plain memory entry (name only) to a time-aware, importance-flagged item that projects into the Agenda surface.

Lists are reusable by design. Items are consumable within a list. The list persists across uses.

Lists are scoped to the household (family). They may be optionally associated with a household Area or a Plan as contextual anchors, but these links are informational — linking a list to a plan does not convert its items to scheduled work, and linking to an Area does not affect ownership or responsibility.

List state is shared in real time across household members — additions, toggles, and updates are visible to all without a manual refresh.

<!-- pdac:cite id="BC-LISTS" digest="sha256:278bab0e63c76bf592a652d1a53fcaf99a6a79cf52153a4d566d26dfcd1bf0e0" -->

<!-- pdac:cite id="TERM-LIST" digest="sha256:6549eba0b2224a4838902f734991430b4f097d5c9d8de8401f67573f4e78ec7a" -->

<!-- pdac:cite id="TERM-LIST-ITEM" digest="sha256:904520339922c3589e43d6ea243a61938b6f5304cb1e30cc8c8b7bffff92aa2e" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:cb2e2207883d40101d5b4ef7b33d79dc2c444d62b2b66c80c5e887b5e938b061" -->

<!-- pdac:cite id="CON-LISTS-ITEM-CAPABILITY-BOUNDARY" digest="sha256:057cef9585d6cb8ab3cba66f55cdb76637abe9b9bb9342a997232b58dba53e15" -->

<!-- pdac:cite id="BR-LISTS-HOUSEHOLD-SCOPED" digest="sha256:7ea7d7ef5ff31e6a1296f023db734a577b07a449f4beb31bb72d31ef9c500072" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:75037c1fcced9208b8cd1c668a98f37b7d742dea8c8f447a482e15bf7e3c76f6" -->

<!-- pdac:cite id="BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT" digest="sha256:4c9723864e62308e6b5a0068872b52fcafd549eebd8837c69d9eef568c10ea42" -->

<!-- pdac:cite id="QR-LISTS-REAL-TIME-SHARING" digest="sha256:bc063af2dad51048a90d0f8d29ee7466d0d74e5ad96462900c5a476996d89455" -->

<!-- pdac:cite id="JRN-LISTS-REUSE-A-LIST" digest="sha256:e59043d8f633b8e68bbfff954803e5f97c46167d42a022ac5b11b7b875603d10" -->

<!-- pdac-drift ids="BR-LISTS-PRIVATE-LIST, FR-LISTS-PRIVATE-LIST, BR-LISTS-HOUSEHOLD-SCOPED" summary="Purpose presents every list as household-scoped and shared with all members; the model lets a list be private to one person (Q-0027)" -->

---

## Requirements

### Requirement: List Creation

A household SHALL be able to create a list with a name as the only required input.

Optional associations at creation: an Area (contextual memory for that Area), a Plan (list is used in context of that plan), and a kind (system-level classification). All optional fields may also be set or changed after creation.

A list is initialized with an empty item collection.

<!-- pdac:cite id="FR-LISTS-CREATE-LIST" digest="sha256:40e9d54e5314ad2e475d7fce3b2a85fa548e4b4928390b1a4cbb5c3889b68c8c" -->

<!-- pdac:cite id="SB-LISTS-CREATE-NAME-ONLY" digest="sha256:debfbf3036eda107b3f1f195d25d5023ed3fa3554f5f9da2bb9f76da13caa8ab" -->

<!-- pdac:cite id="SB-LISTS-CREATE-LINKED-TO-PLAN" digest="sha256:c6282a216c55b5f3878fc6cf6827b2459a67b601f4ff373f32cd996bab2052a5" -->

<!-- pdac:cite id="UC-LISTS-CREATE-LIST" digest="sha256:375c978ff484265047fac5dc6656ddafb9b13474b61d356ce67388071a75ae5e" -->

<!-- pdac:cite id="BR-LISTS-LIST-NAME-REQUIRED" digest="sha256:e8b8f128499fe155698642fddd3002344b0ec947bece7644a30f3d8f54a7b752" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:75037c1fcced9208b8cd1c668a98f37b7d742dea8c8f447a482e15bf7e3c76f6" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:cb2e2207883d40101d5b4ef7b33d79dc2c444d62b2b66c80c5e887b5e938b061" -->

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:54f0abaea8ee6e4683dca671179356b0dc68aeaeab8353b550bfefc66212698c" -->

<!-- pdac:cite id="TERM-LIST" digest="sha256:6549eba0b2224a4838902f734991430b4f097d5c9d8de8401f67573f4e78ec7a" -->

<!-- pdac:cite id="TERM-LIST-KIND" digest="sha256:87ad710f1908d9fcf40c75670966f9780f7f405eb43dc05cb3fe6fbf2fda6672" -->

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

<!-- pdac:cite id="FR-LISTS-BROWSE-LISTS" digest="sha256:dc7bf80f44b5e99a69a0c2449b160a2d5bca9746fb817cffc1e7e28d04c24228" -->

<!-- pdac:cite id="SB-LISTS-BROWSE-ACTIVE" digest="sha256:fa0cf89fdc326f3dad3a6915aca5cdae9a28aa574d3dbc0820c7cf99db9dc0ce" -->

<!-- pdac:cite id="SB-LISTS-BROWSE-EXCLUDES-ARCHIVED" digest="sha256:7e2a42d48e84b2d5e1d6cfada8f22ecf93dae75bb69583d2963a8ba4ddb2c5e4" -->

<!-- pdac:cite id="UC-LISTS-BROWSE-LISTS" digest="sha256:119aa4f67efbc350613fa0f77e48134406a41ffc0e0c53b729e73f3e57d15508" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:71bd5c8a326250a271fca746551c3e38d203d4318eb29bc9ee02dc6b1487b85d" -->

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

<!-- pdac:cite id="FR-LISTS-OPEN-LIST" digest="sha256:2adf8e6854602e5d6029b6cb889800220fed57d862de2bbfa07136611deb8053" -->

<!-- pdac:cite id="SB-LISTS-OPEN-UNCHECKED-FIRST" digest="sha256:6c1e8e0624f0b3efc8eb12a39bc8297df35fd52943cee0474d2bbaacd422f779" -->

<!-- pdac:cite id="UC-LISTS-OPEN-LIST" digest="sha256:245d5390e63e0281cb1cb5c29ef1a9fa25ab39fc2c4cf54b51240b6d5f571dfe" -->

<!-- pdac:cite id="TERM-CHECKED-ITEM" digest="sha256:5556c31585a82994143d5a49b60e7d1c8d9780aede68115e79e8fbaee6499246" -->

#### Scenario: Household opens a list

- GIVEN a list exists with a mix of checked and unchecked items
- WHEN the household retrieves the list detail
- THEN all items are returned
- AND unchecked items appear before checked items

---

### Requirement: List Update

A household SHALL be able to update a list's name and optional associations.

Updatable fields: name, Area association, linked Plan, kind. At least one field must be provided. Fields not included in the request are unchanged. Area association and Plan linkage may be cleared explicitly.

<!-- pdac:cite id="FR-LISTS-UPDATE-LIST" digest="sha256:a4105ede932f96b9ce49aaf4ac170e6fc24535aa956e06ecf7142e34054ef8b9" -->

<!-- pdac:cite id="SB-LISTS-RENAME" digest="sha256:8c38737a493ec6decdcef6879fbeb7a6e2f85adaa8d9dfc52be277a31382325f" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-PLAN-LINK" digest="sha256:4122f6bb3f4af302880e42ca6de41325376b4fe52bc717716233a52f7b9ad8a5" -->

<!-- pdac:cite id="UC-LISTS-UPDATE-LIST" digest="sha256:f49087c1e3e6cc028bd2013b62b7a4c65f17341ea79a44435a1920251609b9ed" -->

<!-- pdac:cite id="BR-LISTS-CONTEXT-LINKS-INFORMATIONAL" digest="sha256:75037c1fcced9208b8cd1c668a98f37b7d742dea8c8f447a482e15bf7e3c76f6" -->

<!-- pdac:cite id="BR-LISTS-LIST-NAME-REQUIRED" digest="sha256:e8b8f128499fe155698642fddd3002344b0ec947bece7644a30f3d8f54a7b752" -->

<!-- pdac:cite id="TERM-LIST-KIND" digest="sha256:87ad710f1908d9fcf40c75670966f9780f7f405eb43dc05cb3fe6fbf2fda6672" -->

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

<!-- pdac:cite id="FR-LISTS-ARCHIVE-LIST" digest="sha256:7204dcb449c16d9bf95653de6d5dab3140f60ad07df5dcc3a31ae877b6e27e57" -->

<!-- pdac:cite id="SB-LISTS-ARCHIVE" digest="sha256:148dd74a469517cc70e4380b54ba5be66496edce6a7358737baf4bbeb42d2a3f" -->

<!-- pdac:cite id="SB-LISTS-ARCHIVE-TWICE-REJECTED" digest="sha256:f0afc89b3c30b90a451656df97693cdf5091388d1ddc0ac7b78323e07702dbec" -->

<!-- pdac:cite id="UC-LISTS-ARCHIVE-LIST" digest="sha256:2395fb97234a2de067a6f26ada8345758c18ccf3ef081d97c9d20a9b04fa6afe" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVE-STATE" digest="sha256:c9572f4c87f0eb313eb0ce9b60311c9d8f81eafe0a394767fea4561592e48171" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVED-READ-ONLY" digest="sha256:5e8bfb0f1ca4b21a79d05c4ea7f94e1a899b3443f8b9625af913ed60dc9f7c70" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:71bd5c8a326250a271fca746551c3e38d203d4318eb29bc9ee02dc6b1487b85d" -->

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

<!-- pdac:cite id="FR-LISTS-RESTORE-LIST" digest="sha256:e891cbbb3a192f687a69b80fbf5040c39616a5c0ada7f4094f40c520533939b9" -->

<!-- pdac:cite id="SB-LISTS-RESTORE" digest="sha256:3487aac1bab98bd2d8e818920ef1f2b1e67626e42306ca610f2650aea37bf8b5" -->

<!-- pdac:cite id="UC-LISTS-RESTORE-LIST" digest="sha256:eced0ea5306e36b24e7383bfe6bb7697d9e911b2a3158553c2014bacee0c78d0" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVE-STATE" digest="sha256:c9572f4c87f0eb313eb0ce9b60311c9d8f81eafe0a394767fea4561592e48171" -->

<!-- pdac:cite id="TERM-ARCHIVED-LIST" digest="sha256:71bd5c8a326250a271fca746551c3e38d203d4318eb29bc9ee02dc6b1487b85d" -->

#### Scenario: Household restores an archived list

- GIVEN an archived list exists
- WHEN the household restores it
- THEN the list appears again in the default active list query
- AND all items are in the same state as when the list was archived

---

### Requirement: Item Addition

A household SHALL be able to add a new item to an active list with only a name.

A new item is always created in an unchecked state. Optional fields at creation: quantity and note. Items are appended in stable order — each new item receives the next position. Importance and temporal fields are not set at item creation; they are applied through dedicated operations after creation.

<!-- pdac:cite id="FR-LISTS-ADD-ITEM" digest="sha256:e8aa749e760b1deb27d59fd5f3e997342ccfd52438cffb1f96e79479275a3ce5" -->

<!-- pdac:cite id="SB-LISTS-ADD-ITEM" digest="sha256:76fc4b35e2d298256f90e2780c892669ff60c867c1319ec47c463b5cec3ad4ab" -->

<!-- pdac:cite id="SB-LISTS-SEQUENTIAL-CAPTURE" digest="sha256:084f626099f7e5566efe9ecbb65bda5c6a31d63d03ea1c9fed0045de260fe92c" -->

<!-- pdac:cite id="UC-LISTS-ADD-ITEM" digest="sha256:c8effd1bb82b42ca0d65ff777b6268c2501387060f57a85bf2af661b0c14aa34" -->

<!-- pdac:cite id="BR-LISTS-NEW-ITEM-UNCHECKED-APPENDED" digest="sha256:4777df4ac2432868485d8e0f3217cc860d76988bf8ad912712ada779661ec493" -->

<!-- pdac:cite id="BR-LISTS-ITEM-NAME-REQUIRED" digest="sha256:34df8f109bcc1480c64730e5bb5bf10a7dcc96602b753c50ba7b25deddedc8bf" -->

<!-- pdac:cite id="BR-LISTS-ARCHIVED-READ-ONLY" digest="sha256:5e8bfb0f1ca4b21a79d05c4ea7f94e1a899b3443f8b9625af913ed60dc9f7c70" -->

<!-- pdac:cite id="QR-LISTS-FRICTIONLESS-CAPTURE" digest="sha256:54f0abaea8ee6e4683dca671179356b0dc68aeaeab8353b550bfefc66212698c" -->

<!-- pdac:cite id="TERM-LIST-ITEM" digest="sha256:904520339922c3589e43d6ea243a61938b6f5304cb1e30cc8c8b7bffff92aa2e" -->

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

At least one of these fields must be provided. Fields not included are left unchanged. Quantity may be cleared explicitly. This operation does not affect checked state, importance, temporal fields, or item order.

<!-- pdac:cite id="FR-LISTS-UPDATE-ITEM" digest="sha256:ffef253a196bbafe63a688a4cea869d3f9a9146ceb230dce59a9448726c092f7" -->

<!-- pdac:cite id="SB-LISTS-RENAME-ITEM" digest="sha256:cbc4a2a83389f96e903c7abb73632e47c8dc11cce7732e3c84e0e88a58a67977" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-QUANTITY" digest="sha256:5d2111f912868ddb4bf2266e152945ac1aa52bfbd306888bef1da93ac89470e4" -->

<!-- pdac:cite id="UC-LISTS-UPDATE-ITEM" digest="sha256:9374470251c52adcbb0149d065d560a5b69e49b3ca0ca3581fbd6dea03e52719" -->

<!-- pdac:cite id="BR-LISTS-ITEM-NAME-REQUIRED" digest="sha256:34df8f109bcc1480c64730e5bb5bf10a7dcc96602b753c50ba7b25deddedc8bf" -->

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

<!-- pdac:cite id="FR-LISTS-TOGGLE-ITEM" digest="sha256:f62cb9ee1d25437277aa435e386caa5a02b6f268803cf70861c8721286e28af5" -->

<!-- pdac:cite id="SB-LISTS-CHECK-ITEM" digest="sha256:5fc60ec7a75826e0db808d5dcc97cb2187c2ea6cac94adfd7f81fef0b65964a4" -->

<!-- pdac:cite id="SB-LISTS-UNCHECK-ITEM" digest="sha256:b47d76803eafcaa2807c5fb4ca3cd6715945ef7c3b4b8460af3b021a9c19cb20" -->

<!-- pdac:cite id="SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA" digest="sha256:63ba6a5bcd5da05939a9be25150e296659891d74f376ca91ff5be8f1414466d8" -->

<!-- pdac:cite id="UC-LISTS-TOGGLE-ITEM" digest="sha256:5cc4db7826b2b31f6e00a93f54f3f1d5e70be7e608f6f4c86cadcbed1816b24f" -->

<!-- pdac:cite id="BR-LISTS-TOGGLE-PRESERVES-ITEM" digest="sha256:e8530110ac8225a196763352f7aa29d44027f7be270e92eb77feb84161eb559e" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:07f2a39c4da3143f7ba1dc285acb840d8a121020a2c68adf87783c23b1e92af9" -->

<!-- pdac:cite id="TERM-CHECKED-ITEM" digest="sha256:5556c31585a82994143d5a49b60e7d1c8d9780aede68115e79e8fbaee6499246" -->

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

<!-- pdac:cite id="FR-LISTS-REMOVE-ITEM" digest="sha256:85ac6d83788ba434bd792f7db6d2b5af4790b4cf01d493425c5a0cdbfff24186" -->

<!-- pdac:cite id="SB-LISTS-REMOVE-ITEM" digest="sha256:6b46f0c26eb5bd4b8b2a89a17e9b4f278e87bbca443cffb79d12fe1297826043" -->

<!-- pdac:cite id="SB-LISTS-REMOVE-DATED-ITEM" digest="sha256:5aa6817b2d78843455635a435736b67dcd85f91ff38dcb213b98456c6e9ffbc5" -->

<!-- pdac:cite id="UC-LISTS-REMOVE-ITEM" digest="sha256:ee30e1a30a9b03fa6c6126c06145bfbfd5d59bbc0590ac69c5c15b6ad32ce57e" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:07f2a39c4da3143f7ba1dc285acb840d8a121020a2c68adf87783c23b1e92af9" -->

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

<!-- pdac:cite id="FR-LISTS-REORDER-ITEMS" digest="sha256:ab648a92c737aad4cb0a2e5c49a5379f69f79f6dec2f6a28d042ad7aa2191ca8" -->

<!-- pdac:cite id="SB-LISTS-REORDER" digest="sha256:badbdbfe8f21dad8b47dd64f436db410d8145850084f97d3032d965ad322d2ab" -->

<!-- pdac:cite id="SB-LISTS-REORDER-MISMATCH-REJECTED" digest="sha256:c06c138867b0776aa11fb2e99827823e201f3ce89ed847ab163c82971724d7dd" -->

<!-- pdac:cite id="UC-LISTS-REORDER-ITEMS" digest="sha256:460b0473750714aa1ace70dd43d5a44add2d20420bbae7e6fa9babb549f52fb6" -->

<!-- pdac:cite id="BR-LISTS-REORDER-FULL-SET" digest="sha256:e8bdf1316756fa650fa405e2d3951dd016da9886dcf510a4b2f45967674ff1f0" -->

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

<!-- pdac:cite id="FR-LISTS-ITEM-IMPORTANCE" digest="sha256:6f2ad263ba0ae54f8a682c9578ec553165f84da6bc547ed31a224ab021d09834" -->

<!-- pdac:cite id="SB-LISTS-STAR-ITEM" digest="sha256:62c8fdf2528be0ee4c295066bc8d3034b4d083f5d12becd4ca52697b0858098e" -->

<!-- pdac:cite id="SB-LISTS-STAR-IDEMPOTENT" digest="sha256:1f7b8203d39039b66d14a33b11f4609d30814764e02adca9bbd29bdcab4a6f36" -->

<!-- pdac:cite id="UC-LISTS-SET-ITEM-IMPORTANCE" digest="sha256:0f88892c503db136b57d4196f34f7edfb6b7e591e816411da99788cd10f84b95" -->

<!-- pdac:cite id="BR-LISTS-IMPORTANCE-BINARY" digest="sha256:8f9276f3e7b87e7f2ad2c59437da11a5ec0481cdaa7f97e520c6f8a91a8fe6c3" -->

<!-- pdac:cite id="TERM-ITEM-IMPORTANCE" digest="sha256:10b6e633466cc85d1aee7ab7f7d74e519279a86581c1f715e2cb4956e1437a10" -->

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

<!-- pdac:cite id="FR-LISTS-SET-ITEM-TIMING" digest="sha256:d7ebe6861d147b98d3f0b647ac8ab6123a20cea7ff52e516210fe306593e9a93" -->

<!-- pdac:cite id="SB-LISTS-SET-DUE-DATE" digest="sha256:147ea57d7976d23536d9872f2464ada239fae8d12932767343fc2ea3caa0b74f" -->

<!-- pdac:cite id="SB-LISTS-SET-REMINDER-ONLY" digest="sha256:7b45148dff325bf184897a76389b4abb1ea71823325ce9d76c4a0fae40068cc3" -->

<!-- pdac:cite id="SB-LISTS-SET-REPEAT-ONLY" digest="sha256:79fecc7cdf6782364cc29d24cc716e428979a034d0ae47760c323367d5a4a2d7" -->

<!-- pdac:cite id="UC-LISTS-SCHEDULE-ITEM" digest="sha256:771638ba8221f8c57b50e04a490d9960d1600f90e549c31d6e14556d09c29e00" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:40002c49ac40b5b30be14b9d66152d74c7c6db734817ff70ba4976c4849778fe" -->

<!-- pdac:cite id="BR-LISTS-ITEM-IS-NOT-A-TASK" digest="sha256:cb2e2207883d40101d5b4ef7b33d79dc2c444d62b2b66c80c5e887b5e938b061" -->

<!-- pdac:cite id="TERM-ITEM-TEMPORAL-FIELDS" digest="sha256:138ea408865568c2d484dcdef52b9395e05a4f103ce7fc9462e9deead72e484d" -->

<!-- pdac:cite id="TERM-ITEM-REPEAT-RULE" digest="sha256:d93b02a933295822a4fd3d73a0f27d879475d21592ef5edd45587d7ef5cf12a1" -->

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

<!-- pdac:cite id="FR-LISTS-CLEAR-ITEM-TIMING" digest="sha256:dac218145bbf6f152b37ca13e0c90761046bf9141f5cec589137e62d80d0ebda" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-TIMING" digest="sha256:3fe8223fd91fa8cd3f7aebcf7f9b063ec50a2cf5c1a258bcd472cc8579918a2b" -->

<!-- pdac:cite id="SB-LISTS-CLEAR-TIMING-NOOP" digest="sha256:105db06995bea76e40fe3e50b19465680895c07e998d4cf2185d49c90c91bc86" -->

<!-- pdac:cite id="UC-LISTS-CLEAR-ITEM-SCHEDULE" digest="sha256:1c2d65f129e1410eab1dd7136e4049a598402c85ab6205025acdb3256c9de277" -->

<!-- pdac:cite id="TERM-ITEM-TEMPORAL-FIELDS" digest="sha256:138ea408865568c2d484dcdef52b9395e05a4f103ce7fc9462e9deead72e484d" -->

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

<!-- pdac:cite id="FR-LISTS-AGENDA-PROJECTION" digest="sha256:97074fff7f6f05b914ce4619df4ea615d6d3b8198c29f7a9c85b9cd0a95ffe0b" -->

<!-- pdac:cite id="SB-LISTS-DUE-TODAY-IN-AGENDA" digest="sha256:b6aeb0a0e614423de4e288251c78ab684092caff4e0281e870e87e902975bab3" -->

<!-- pdac:cite id="SB-LISTS-CHECKED-ITEM-STAYS-IN-AGENDA" digest="sha256:63ba6a5bcd5da05939a9be25150e296659891d74f376ca91ff5be8f1414466d8" -->

<!-- pdac:cite id="SB-LISTS-CLEARED-ITEM-LEAVES-AGENDA" digest="sha256:9f66ef5bca4eecb07050a78ed725f41873bd84e1422ad2ae6d4cd783dc29c0ff" -->

<!-- pdac:cite id="UC-LISTS-SEE-ITEMS-IN-AGENDA" digest="sha256:ee7a292721be6b4b2df1d3de830f3918089d760b22e49883e906b03b657c7e80" -->

<!-- pdac:cite id="BR-LISTS-AGENDA-PROJECTION" digest="sha256:07f2a39c4da3143f7ba1dc285acb840d8a121020a2c68adf87783c23b1e92af9" -->

<!-- pdac:cite id="BR-AGENDA-PROJECTION-READ-ONLY" digest="sha256:652b5a6581672dfbabf8e5da42d010cc34d51bbcbf5b2bc73fac9273bb631337" -->

<!-- pdac:cite id="BR-LISTS-TEMPORAL-FIELDS-INDEPENDENT" digest="sha256:40002c49ac40b5b30be14b9d66152d74c7c6db734817ff70ba4976c4849778fe" -->

<!-- pdac:cite id="TERM-PROJECTED-LIST-ITEM" digest="sha256:c8ace0e5be8d059740bfd3edd63eb5f8b101c2687ffcccbc7f918c9e5a864fd7" -->

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

The context document (`docs/_legacy/04_contexts/shared-lists.md`) and the item model document use "Shared Lists" and "SharedList" throughout. The product surface (`00_product/surfaces/lists.md`) and feature specs use "Lists". This spec uses "Lists" as the product-facing term. The transition is incomplete in the domain docs; "SharedList" remains the aggregate name in the domain model.

### Contradiction: `repeat` independence — Resolved

The `set-item-temporal` feature spec previously stated that `repeat` requires a `dueDate` to be present, contradicting the item model document which states that `repeat` is independently sufficient for Agenda projection. This contradiction is resolved by `docs/_legacy/01_system/system-spec.md`, which explicitly states: "Repeat on a list item may be set independently of due date. Repeat is itself a temporal anchor sufficient for Agenda projection." The item model canonical position is confirmed. The `set-item-temporal` feature spec constraint was stale.

### `get-list-detail` item fields

The `get-list-detail` feature spec defines the per-item result shape as: name, quantity, note, checked, order — and explicitly excludes temporal and importance fields. This contradicts the item capability model, which makes these fields part of the item's full state. The item model document is marked as locking canonical state before implementation. The detail spec appears to predate it and should be updated to include importance and temporal fields in the item result.

### Partial temporal field clearing

`ClearSharedListItemTemporal` clears all three fields atomically. To clear only specific temporal fields individually, the `SetSharedListItemTemporal` operation is used by setting specific fields to null. This asymmetry is intentional per the source.

### Shopping list creation from Meal Planning

The context document notes that the Lists context receives shopping lists generated from Meal Planning as a `List` of kind `shopping`. The reception side behavior (how a shopping list lands in Lists) is owned by the Meal Planning spec, not this spec. The Lists context treats a shopping list as a regular list once created.

---

## Source References

- `docs/_legacy/04_contexts/shared-lists.md`
- `docs/_legacy/04_contexts/shared-lists-item-model.md`
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
