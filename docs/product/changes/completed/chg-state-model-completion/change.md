---
id: CHG-STATE-MODEL-COMPLETION
type: product-change
title: Complete household access and approved state lifecycles
status: applied
base-revision: 430a49a89482609d52417a32f5dc7064fe199c71
operations:
  add:
    - LC-MEAL-PLAN
    - LC-MEMBER-ACCESS
    - SB-CALENDAR-FULL-RELOAD-STAYS-SYNCING
    - SB-FAMILY-ACCEPT-ACCESS-INVITATION
    - SB-FAMILY-REPLACE-ACCESS-INVITATION
    - SB-FAMILY-REVOKE-ACCESS-INVITATION
    - SB-INBOX-APPLIED-PROPOSAL-CANNOT-EXECUTE-AGAIN
    - SB-INBOX-REAPPROVE-CORRECTED-PROPOSAL
    - SB-INBOX-RECOVER-UNAPPLIED-FAILURE
    - SB-MEALS-COMPLETE-PLAN
    - SB-MEALS-COMPLETED-PLAN-CANNOT-REOPEN
    - SB-MEALS-WEEK-END-KEEPS-ACTIVE
    - UC-MEALS-COMPLETE-PLAN
  modify:
    - BR-INBOX-REVIEW-AND-EXECUTION
    - BR-MEALS-PLAN-LIFECYCLE
    - FR-CALENDAR-SHOW-CONNECTION-STATUS
    - FR-FAMILY-MANAGE-MEMBER-ACCESS
    - FR-INBOX-REVIEW
    - FR-MEALS-PLAN-LIFECYCLE
    - FR-WEB-CALENDAR-CONNECTIONS-SECTION
    - JRN-CHAT-REVIEW-HOUSEHOLD-ACTION
    - LC-EXTERNAL-CALENDAR-CONNECTION
    - LC-INBOX-PROPOSAL
    - SB-FAMILY-MANAGER-DISABLES-ACCESS
    - SB-FAMILY-MANAGER-ENABLES-ACCESS
    - SB-FAMILY-MANAGER-PROVISIONS-ACCESS
    - SB-INBOX-INVALID-PROPOSAL
    - SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE
    - SB-MEALS-PROMOTE-TO-ACTIVE
    - TERM-EXTERNAL-CALENDAR-CONNECTION
    - TERM-EXTERNAL-CALENDAR-SYNC
    - TERM-MEAL-PLAN-STATUS
    - UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS
    - UC-INBOX-CAPTURE-AND-REVIEW
  remove: []
unaffected:
  - id: QR-INBOX-PRIVATE-HOUSEHOLD-DATA
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: applies-to
    digest: sha256:90b04c52585e12c8c0083b41322685067f69f32783fd280db0e895f7714debb3
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-CALENDAR-AUTH-EXPIRED
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:a22faad8c9b776b655d2a9815f86f0c48acfaa62ae32b42013b195e70a3c3b62
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-CALENDAR-DISCONNECT
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:08ccad7fc87478aee53ebac67db39444dd69f4badb02aff22d6b89e14f8b8e06
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-CALENDAR-START-SYNC
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:996e89b990dfbdefd982d6780bbf9852fe2974ee74b4182695f7be51bc974cb3
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-CALENDAR-SYNC-FAILURE
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:419948259d4b694c298500f74d7182ac0626ed6fd3f87af7f3059b1e36141e95
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-CALENDAR-SYNC-PARTIAL-FAILURE
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:b881b21e26358be7d645b83ee3908403d9a02d0ff581ee2a073eb07e16383d10
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-CALENDAR-SYNC-SUCCESS
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:cbbd36431def80c650c335aa667b2744301dbd0bf3d4ab7fc337faffd251f50e
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-WEB-CONFIRM-DISCONNECT
    cause: LC-EXTERNAL-CALENDAR-CONNECTION
    relationship: covers-transition.lifecycle
    digest: sha256:538194d0e6a28b3756af7560f8ed6ed72894fba8c75be6989ea7d0f38cfded60
    cause-digest: sha256:04f6d9502ca9d27cab5239ef3a179979ad42aa0a2b4cf67d11d87916f80ce7ec
    reason: The covered transition keeps the same trigger and result. The example never relies on Needs attention or Rehydrating as its starting or ending state, so removing those catalog entries does not change this example.
  - id: SB-INBOX-APPLY-PROPOSAL
    cause: LC-INBOX-PROPOSAL
    relationship: covers-transition.lifecycle
    digest: sha256:3e4088750fb7da5d4ca71595e53ad7e5aee7d83f8a1ba89bf35ccd49051a762c
    cause-digest: sha256:6a279f3f478b30f7097d5ad9356fe3cd391905b302d9a7b82e88e0c47e494599
    reason: Approval, successful application and rejection keep their existing state changes. The newly added recovery transition applies to failed unapplied actions, outside this example's successful decision path.
  - id: SB-INBOX-APPROVE-PROPOSAL
    cause: LC-INBOX-PROPOSAL
    relationship: covers-transition.lifecycle
    digest: sha256:089d5bad9e4c0cdbd7a78d761442bfaa43abf63ab70f1570e0338665ab9fc99f
    cause-digest: sha256:6a279f3f478b30f7097d5ad9356fe3cd391905b302d9a7b82e88e0c47e494599
    reason: Approval, successful application and rejection keep their existing state changes. The newly added recovery transition applies to failed unapplied actions, outside this example's successful decision path.
  - id: SB-INBOX-REJECT-PROPOSAL
    cause: LC-INBOX-PROPOSAL
    relationship: covers-transition.lifecycle
    digest: sha256:476c41edabc786729668b9c7f05a72c350b3465c3d598b079413fd9940a46100
    cause-digest: sha256:6a279f3f478b30f7097d5ad9356fe3cd391905b302d9a7b82e88e0c47e494599
    reason: Approval, successful application and rejection keep their existing state changes. The newly added recovery transition applies to failed unapplied actions, outside this example's successful decision path.
  - id: FR-MEALS-SLOT-ASSIGNMENT
    cause: BR-MEALS-PLAN-LIFECYCLE
    relationship: derived-from
    digest: sha256:7c02ee787691996b2cafed0e60c636487f906536247d3f5960e2beb8c18d786b
    cause-digest: sha256:5fa200b2cfb67f04c6205fc17a7d4b8b1463ff75f79af75891ae8337a93c605d
    reason: The added manual completion rule preserves Draft creation, explicit activation, slot mutation in Draft or Active and refusal of Completed mutation. This artifact covers those existing paths and does not prescribe an automatic completion trigger or reopening.
  - id: FR-INBOX-CAPTURE
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: derived-from
    digest: sha256:522b9c28e76f8f2f7c113be168f53020be4cbd7abdbfaea52b28577d98a9e2db
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: FR-INBOX-INTERPRET
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: derived-from
    digest: sha256:8655bde49845f4278b7ad37baeab11421c4ae263138f278a4a902d6aef1fcee8
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: UC-MEALS-APPLY-TEMPLATE
    cause: BR-MEALS-PLAN-LIFECYCLE
    relationship: governed-by
    digest: sha256:2d62181e6fc6fe0dedc0d942713adf63104020c991ef3db8b32acc114d53ce66
    cause-digest: sha256:5fa200b2cfb67f04c6205fc17a7d4b8b1463ff75f79af75891ae8337a93c605d
    reason: The added manual completion rule preserves Draft creation, explicit activation, slot mutation in Draft or Active and refusal of Completed mutation. This artifact covers those existing paths and does not prescribe an automatic completion trigger or reopening.
  - id: UC-MEALS-COPY-PREVIOUS-WEEK
    cause: BR-MEALS-PLAN-LIFECYCLE
    relationship: governed-by
    digest: sha256:1afa37619a41616c6389f594c9cb4364ff6e6bdacc7c5744d3e15fbc0ae62ec6
    cause-digest: sha256:5fa200b2cfb67f04c6205fc17a7d4b8b1463ff75f79af75891ae8337a93c605d
    reason: The added manual completion rule preserves Draft creation, explicit activation, slot mutation in Draft or Active and refusal of Completed mutation. This artifact covers those existing paths and does not prescribe an automatic completion trigger or reopening.
  - id: UC-MEALS-CREATE-PLAN
    cause: BR-MEALS-PLAN-LIFECYCLE
    relationship: governed-by
    digest: sha256:66ac90fb2f2596dc8da7299347b9a99411dfe406c769f49eeb2d5a0f409b15bd
    cause-digest: sha256:5fa200b2cfb67f04c6205fc17a7d4b8b1463ff75f79af75891ae8337a93c605d
    reason: The added manual completion rule preserves Draft creation, explicit activation, slot mutation in Draft or Active and refusal of Completed mutation. This artifact covers those existing paths and does not prescribe an automatic completion trigger or reopening.
  - id: UC-MEALS-PROMOTE-PLAN
    cause: BR-MEALS-PLAN-LIFECYCLE
    relationship: governed-by
    digest: sha256:b26eecfdff35bb0f581dd955e0960ccc97969c86f451c03d7779b57679eb3c1d
    cause-digest: sha256:5fa200b2cfb67f04c6205fc17a7d4b8b1463ff75f79af75891ae8337a93c605d
    reason: The added manual completion rule preserves Draft creation, explicit activation, slot mutation in Draft or Active and refusal of Completed mutation. This artifact covers those existing paths and does not prescribe an automatic completion trigger or reopening.
  - id: UC-MEALS-UPDATE-SLOT
    cause: BR-MEALS-PLAN-LIFECYCLE
    relationship: governed-by
    digest: sha256:a659fa4c170ec938919501335736ec5ea87ea35c81cc207d793db62e5a6c9d74
    cause-digest: sha256:5fa200b2cfb67f04c6205fc17a7d4b8b1463ff75f79af75891ae8337a93c605d
    reason: The added manual completion rule preserves Draft creation, explicit activation, slot mutation in Draft or Active and refusal of Completed mutation. This artifact covers those existing paths and does not prescribe an automatic completion trigger or reopening.
  - id: SB-CHAT-STAGE-PLAN-PROPOSAL
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:2f4cc65798d5c767e50e8814ccaba500477856b87427bc6bc5fa7dc5f766c15b
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-INBOX-APPLY-PROPOSAL
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:3e4088750fb7da5d4ca71595e53ad7e5aee7d83f8a1ba89bf35ccd49051a762c
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-INBOX-APPROVE-PROPOSAL
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:089d5bad9e4c0cdbd7a78d761442bfaa43abf63ab70f1570e0338665ab9fc99f
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-INBOX-CAPTURE-NOTE
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:760150bc6aa3364ef8a325cea1c5a930535c0a548dfbc4e5aae8376439897d7c
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-INBOX-INTERPRET-NOTE
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:2e224a21b4ec2829332dcb5728389ee770b31ba4701a69f0a3ce290925878ba4
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-INBOX-REJECT-PROPOSAL
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:476c41edabc786729668b9c7f05a72c350b3465c3d598b079413fd9940a46100
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-INBOX-UNAUTHORIZED-PROPOSAL
    cause: BR-INBOX-REVIEW-AND-EXECUTION
    relationship: illustrates
    digest: sha256:c799f331deda3233b0c67382da1aa6d29ebdf2b6abf084ae30c0255f4378b8f6
    cause-digest: sha256:4cefc25292234d58d9972915de22d8811ee3bb7b3989512d161482347dc0b267
    reason: The new rules govern failed unapplied execution, explicit reapproval and prevention of repeated Applied execution. This example covers capture, staging, an initial decision, successful application or denied authorization; its given conditions and results remain valid.
  - id: SB-CHAT-STAGE-PLAN-PROPOSAL
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:2f4cc65798d5c767e50e8814ccaba500477856b87427bc6bc5fa7dc5f766c15b
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-INBOX-APPLY-PROPOSAL
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:3e4088750fb7da5d4ca71595e53ad7e5aee7d83f8a1ba89bf35ccd49051a762c
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-INBOX-APPROVE-PROPOSAL
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:089d5bad9e4c0cdbd7a78d761442bfaa43abf63ab70f1570e0338665ab9fc99f
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-INBOX-CAPTURE-NOTE
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:760150bc6aa3364ef8a325cea1c5a930535c0a548dfbc4e5aae8376439897d7c
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-INBOX-INTERPRET-NOTE
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:2e224a21b4ec2829332dcb5728389ee770b31ba4701a69f0a3ce290925878ba4
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-INBOX-REJECT-PROPOSAL
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:476c41edabc786729668b9c7f05a72c350b3465c3d598b079413fd9940a46100
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: SB-INBOX-UNAUTHORIZED-PROPOSAL
    cause: UC-INBOX-CAPTURE-AND-REVIEW
    relationship: illustrates
    digest: sha256:c799f331deda3233b0c67382da1aa6d29ebdf2b6abf084ae30c0255f4378b8f6
    cause-digest: sha256:7324aebc6f7b3d23d24faa761084db6c9df72468cf535df6f9967d96589eac20
    reason: The use case adds recovery for failed unapplied actions. This linked artifact covers capture, interpretation, successful review, rejection or existing household privacy; those paths and access boundaries remain unchanged. It declares no conflicting failure state or retry policy.
  - id: BR-CALENDAR-DEFAULT-REFRESH-HOURLY
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:c575f89178139004976122190d8a13c8136f3992a7a9190d2fd419859c41f54a
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-CALENDAR-DISCONNECT-IS-LOCAL
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:4b321feb9a5f47fbcc724b1e6d70a152b70dc67343f5a0d518470aaeb67f19d8
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:a3c89a729bbc1b61d0b8104ceacce71dc29a6e915ab2a90834c869b72c0a9da8
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-CALENDAR-NO-CONCURRENT-SYNC
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:cb91537c8a00c86ee5a8efeb0ae78ae4d5c94f112a9f7445c4792e78295b0d78
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:7ebb4f852677a608d2037cf65b1910e49d3a2e4ee13cb70e59a83cf525777e94
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:556b0ae693f9cb4396113be0e3c161fd22a811c5d94db7d976635570241ad1f1
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-WEB-SETTINGS-STAYS-CONFIGURATION
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:359161ae6d2ae57e66e5f0e4d264593cac97a51760fdf377c0018076b62037f7
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: CON-CALENDAR-OUTLOOK-PULL-ONLY
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:c575d9e19b4520f13a09facea0c1196b6a42ec1f4554865cdb470485d46eb4b0
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-BACKGROUND-REFRESH
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:ba16ad06b77cfb02dcfcbee49418c1a7bf18251f91c4bf5858a6209cf87e59b4
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:cce36628f25ef3a79295b09126348ef04778f8fac999eaddaa4c80341fbc1a83
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-CONNECT-OUTLOOK
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:50cfc7aa037888b885e20f91a1a152ec089c55f4f6fa051fcb2d31a8a227a768
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:a765e9f46bd2d5de55ff5c2e50daa4ac2eaef58df4ce44f8868cda2c9c2b4f76
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-MANAGER-CONNECTION-SUMMARY
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:da1369b92a49ecee17a11e5adeb7c9ba3cc14b89d1e87e989cc3ef0c906263ba
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-SYNC-ALL-MY-CALENDARS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:00d6307f6e04781deae485202279fadb2c0840b08267d96f71e1d8c40f3fc62b
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-WEB-CONFIRM-DISCONNECT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:1cbeb290a7623298a0504c813901a94e5f7e4d678de0dd33b13e9b659f85d640
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: FR-WEB-SETTINGS-SURFACE
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:ded16890d8ee378ace6a8f2e67f815920b569ec20eef0904e177614a0eecb44d
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-AUTH-EXPIRED
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:a22faad8c9b776b655d2a9815f86f0c48acfaa62ae32b42013b195e70a3c3b62
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:ac3ec6421a6dcf9c4c0f0a07350c93e5be1f7dc0faa0400a1b3b33d9d9e48404
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-CONFIGURE-SELECTION-AND-HORIZON
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:1d30d2d05e242313bb1b6df92582aabf7d14327baa86c663f490a8c13aa9ce43
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-CONNECT-DUPLICATE-ACCOUNT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:f3900665713dc2f78171d2306284576fe3fdc4e676b9757e73af1b3bd2ee2949
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-CONNECT-MISSING-ACCESS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:736cc5226870fe1cda9a6cc175028c4642e3be9205a3e9a6da387857b4f20a3b
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-CONNECT-OUTLOOK
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:1b65fce3b6a17f8b0aafbe33bb30a4b9ae43e0cc5fa51d39a2bba8fa5489c346
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-DESELECT-FEED
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:90876b7145514f5b21efdc45116772200cb482f879c93de9394ccc68f7d610be
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-DISCONNECT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:08ccad7fc87478aee53ebac67db39444dd69f4badb02aff22d6b89e14f8b8e06
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-DISCONNECT-UNKNOWN
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:96cbddcb5828d1d7b4bb861ab0218a4e981cc751ea06225292b0e2f813e6537a
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-HORIZON-CHANGE-RELOADS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:32a934be9368ab2a4f217b0efdb3b31f5a6c73abbdc43ef10da1da29a3cbf5d4
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-MANUAL-SYNC
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:d2bb53909bbecb5dff7cfdc073dbd4f73fda69653322c0044b6785aadd914c3e
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-REFRESH-SKIP-FRESH
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:adcf47457405c4c43919233af05a0fe529dbdcaec2aab1b5a8cc84f76e183293
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-REFRESH-STALE
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:2cf9fb91fc8460221c43e1f02ce2454e619ae4a3416dbbab49cc6165f12b4e8a
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-START-SYNC
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:996e89b990dfbdefd982d6780bbf9852fe2974ee74b4182695f7be51bc974cb3
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-SYNC-CONCURRENT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:f008c597906fc26b4326b2c9d9fdfe0f441b312037e758cb88068188187e362b
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-SYNC-FAILURE
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:419948259d4b694c298500f74d7182ac0626ed6fd3f87af7f3059b1e36141e95
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-SYNC-INVALID-STATE
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:9605a38c6c360385f8371ef2b3f437b6866427195a5890a2632541ff897bdfd9
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-SYNC-NO-FEEDS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:48bc3d8c7c4a4c71c9c2b94a5752f6dc987819b03b1bdb85c39083dee5dd94e9
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-SYNC-PARTIAL-FAILURE
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:b881b21e26358be7d645b83ee3908403d9a02d0ff581ee2a073eb07e16383d10
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-SYNC-SUCCESS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:cbbd36431def80c650c335aa667b2744301dbd0bf3d4ab7fc337faffd251f50e
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-CALENDAR-UNSUPPORTED-HORIZON
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:0ef2bf7bdb22fe129bb9ccdf5a9b808c2213742359b664c6499e472b5b3da8a9
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-WEB-CONFIRM-DISCONNECT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:538194d0e6a28b3756af7560f8ed6ed72894fba8c75be6989ea7d0f38cfded60
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-WEB-REQUEST-DISCONNECT
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:e0850b11d01addfc8ff54d1986ecd5fba67f721608a5f0bd909c1b5ebe63a00b
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: SB-WEB-SETTINGS-OPENS-ON-PROFILE
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:d1d2c1d3321d62776397d526c6fb7c76cdd39fd92371b07a34d4359ffef62aec
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: TERM-EXTERNAL-CALENDAR-FEED
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:5624402938f25e7089946fa295798b0c0152942eb66ddf9ec900619d2d0a84ea
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: TERM-SYNC-HORIZON
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:d5e8c5b396642312a552935a31f6134a752040f75fb821ceb329e545f6aac81f
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:eaf278bf6b3a77c0dc9ff934b444ab82b5aa23dac9c882f3c16ed698f1f410ff
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-CALENDAR-CONNECT-OUTLOOK
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:2b5979801bddd1324f3c23b0958b000afe550e98e17a11ce792c2ea54235c034
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:3a3e5fa7533474185fbda7d556bb496ff71feb16302a5e219ff587eba5a386bc
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:b9b47a9df92b0d6a7d456d2831e4359e16c414180f0ec2ccb000906a54c94ab5
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-CALENDAR-REVIEW-HOUSEHOLD-CONNECTION-STATUS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:9fc803df49ea02a3ea910621573a8b7758008afdb6a8f6b4ca54c63f8783f7ce
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:20397f50049cb36f540d4e65b7989c0f38be858535349e86daf4790319b37fe4
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: UC-WEB-MANAGE-SETTINGS
    cause: TERM-EXTERNAL-CALENDAR-CONNECTION
    relationship: uses-terms
    digest: sha256:7e168c902564d7bc6f54495ca764cc3cb6e7976fb327cba962a3176fad2ce07a
    cause-digest: sha256:71fd7f91ddaeb838b698bee045adaad8e95f3790cd3637633ab0fb9ff4af6671
    reason: The term retains member ownership, provider identity, selected feeds, read-only imports and configuration meaning. This artifact uses that concept without prescribing the two retired state names; its own rules remain valid under the seven-state catalog.
  - id: BR-CALENDAR-DEFAULT-REFRESH-HOURLY
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:c575f89178139004976122190d8a13c8136f3992a7a9190d2fd419859c41f54a
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: BR-CALENDAR-HORIZON-CHANGE-RELOADS
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:79b8c9607677595440cf3ec07c3501c38a75c4a6fdb12a17365fe24803cbcfc8
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: BR-CALENDAR-NO-CONCURRENT-SYNC
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:cb91537c8a00c86ee5a8efeb0ae78ae4d5c94f112a9f7445c4792e78295b0d78
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: BR-CALENDAR-REFRESH-FAILURES-ISOLATED
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:5b87e85948d6eb058eacf38cc24a2ee1a5b8afe75104318de9d208a7878366fe
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: CON-CALENDAR-OUTLOOK-PULL-ONLY
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:c575d9e19b4520f13a09facea0c1196b6a42ec1f4554865cdb470485d46eb4b0
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: FR-CALENDAR-BACKGROUND-REFRESH
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:ba16ad06b77cfb02dcfcbee49418c1a7bf18251f91c4bf5858a6209cf87e59b4
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: FR-CALENDAR-SYNC-ALL-MY-CALENDARS
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:00d6307f6e04781deae485202279fadb2c0840b08267d96f71e1d8c40f3fc62b
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:ac3ec6421a6dcf9c4c0f0a07350c93e5be1f7dc0faa0400a1b3b33d9d9e48404
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-HORIZON-CHANGE-RELOADS
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:32a934be9368ab2a4f217b0efdb3b31f5a6c73abbdc43ef10da1da29a3cbf5d4
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-MANUAL-SYNC
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:d2bb53909bbecb5dff7cfdc073dbd4f73fda69653322c0044b6785aadd914c3e
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-REFRESH-SKIP-FRESH
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:adcf47457405c4c43919233af05a0fe529dbdcaec2aab1b5a8cc84f76e183293
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-REFRESH-STALE
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:2cf9fb91fc8460221c43e1f02ce2454e619ae4a3416dbbab49cc6165f12b4e8a
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-SYNC-CONCURRENT
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:f008c597906fc26b4326b2c9d9fdfe0f441b312037e758cb88068188187e362b
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-SYNC-INVALID-STATE
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:9605a38c6c360385f8371ef2b3f437b6866427195a5890a2632541ff897bdfd9
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: SB-CALENDAR-SYNC-NO-FEEDS
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:48bc3d8c7c4a4c71c9c2b94a5752f6dc987819b03b1bdb85c39083dee5dd94e9
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:b9b47a9df92b0d6a7d456d2831e4359e16c414180f0ec2ccb000906a54c94ab5
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
    cause: TERM-EXTERNAL-CALENDAR-SYNC
    relationship: uses-terms
    digest: sha256:20397f50049cb36f540d4e65b7989c0f38be858535349e86daf4790319b37fe4
    cause-digest: sha256:2e1eed83b3396992d892883d9f0efd7616821812d36bd069f085960f95756bea
    reason: A fresh reload retains its existing bounded-import behavior and runs within Syncing. This artifact covers triggering, concurrency, horizon, failure isolation or imported results and requires no separate Rehydrating product state.
  - id: BR-RECIPES-DELETE-GUARD
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:2e15467fd305be37d291774e1e09606ef41448d1713b4f98d5ff2cbce2ada86e
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: FR-RECIPES-DELETE
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:7ad724d99da2ba8bd340eedddcf5d47e338caef5ccfecbf0a3628252da8e6015
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: SB-RECIPES-DELETE-BLOCKED-BY-ACTIVE-PLAN
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:50a9c1d606a609b8e3c5a17c5ae3369c8a19c431ac92476a053da273eae03afd
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: SB-RECIPES-DELETE-USED-BY-COMPLETED
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:ea2dfb9d2504c56d7bf34cf6e64b08158d29c6e803442fd1089954a06c83241a
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: SB-RECIPES-DELETE-USED-BY-DRAFT
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:f11150bc5920881864826435886fc7d05bedb13bb0181ab1c2baf06b5ea18fc4
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: SB-RECIPES-DELETE-USED-BY-MIXED
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:706527d485ae835661411efe9c2f5a42be0ba2d66e482aa6355d802facaba4dd
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: TERM-MEAL-PLAN
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:8c7a85c9b75cec9a89206b938f18e09ff3ad0e373eef27c41e164c3068d4b886
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: UC-MEALS-CREATE-PLAN
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:66ac90fb2f2596dc8da7299347b9a99411dfe406c769f49eeb2d5a0f409b15bd
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: UC-MEALS-PROMOTE-PLAN
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:b26eecfdff35bb0f581dd955e0960ccc97969c86f451c03d7779b57679eb3c1d
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: UC-RECIPES-DELETE-RECIPE
    cause: TERM-MEAL-PLAN-STATUS
    relationship: uses-terms
    digest: sha256:47bb5bbb4aaa4ff7c6a788ca533d974aa3fa9561177752b8ca6dfc7c55ff8b56
    cause-digest: sha256:cfa9f2bb625b6ebbccae3befb45dedc39166e7ad295a4914fe33b24607f9e7f6
    reason: Draft, Active and Completed keep their existing editing and recipe-deletion semantics. The term now supplies the owner-approved manual completion trigger; this artifact specifies no competing trigger or reopening policy.
  - id: FR-MEALS-SLOT-ASSIGNMENT
    cause: SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE
    relationship: verification[].scenario-ref
    digest: sha256:7c02ee787691996b2cafed0e60c636487f906536247d3f5960e2beb8c18d786b
    cause-digest: sha256:0921f8b53e4f37e808c5b7afa96cd591ade940a41097e5f654dba0cd4e82a4b4
    reason: The example still refuses slot mutation on a Completed plan. Its boundary now records the approved manual completion trigger, leaving the slot-assignment requirement satisfied.
---

## Problem

Household access and Meal Plans have state rules without native lifecycles. Meal Plan completion and Inbox failure recovery need product decisions. The calendar status catalog includes two states with no entry path.

## Intended Product Outcome

Access has its existing four states and five transitions. Meal Plans move Draft to Active to Completed through explicit member actions. Finish week closes an Active plan as read-only; it never runs automatically, and Completed cannot reopen. A failed, unapplied Inbox action returns its proposal to Pending with details and its reason; correction and another explicit approval are available, with no automatic retry or repeated execution of an Applied proposal. Calendar has seven product states; a full reload stays Syncing and failures carry their reason.

## Rationale

Juan asked for concrete questions with the simplest reasonable option recommended. On 2026-11-14, he answered "Las tres recomendadas" to these exact choices:

1. "Cierre manual, sin reapertura": a member chooses Finish week (Finalizar semana) to complete an Active meal plan, without automatic weekly closure.
2. "Corregir y volver a aprobar": keep a failed, unapplied proposal Pending, retain its details, show the reason, and require another explicit approval. Applied proposals cannot execute again, and there are no automatic retries.
3. "Simplificar el catálogo": retire Needs attention and Rehydrating as independent product states; show failures under Failed or Authorization expired and keep a full reload within Syncing.

The Meal Planning decision resolves the completion question deferred in Q-0033 while preserving explicit activation. The Calendar decision supersedes the two extra states in Q-0049. The access lifecycle formalizes previously accepted invitation, identity and manager rules. The owner response authorizes these model changes; PR acceptance and application delivery remain separate.

## Affected Product Areas

Household and member access, Meal Planning, Inbox, external Calendar connections and their existing product documentation consumers.

## Open Questions

None for the three choices above. The four separate low-confidence recovered claims and the separate active Roadmap proposal retain their existing questions. Runtime delivery work is identified in the implementation notes.

## Product Acceptance

Native lifecycles and examples express the approved choices. Each state-changing transition has a positive example. Negative examples preserve authorization, terminal-state and repeated-execution boundaries. The calendar graph no longer contains the two unreachable states. Existing FR-to-UC links are preserved; the existing meal lifecycle requirement gains one link to the newly approved completion use case.

## Out of Scope

Runtime implementation, deployment, first-administrator policy changes, new invitation expiration rules and the four unrelated low-confidence claims.
