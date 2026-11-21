<!-- pdac-scope: cited -->

# Implementation verification for issue #358

The table binds the two accepted lifecycles and 26 structured behaviours to executable assertions. Domain theories exercise both one-time outcomes, both cancellation source states, every declared action from terminal states, and suspended-action guards. API tests supply real household authorization and persisted state; PostgreSQL tests use fresh contexts and observe competing writers waiting on `pg_locks`, then assert one winning outcome and no extra history. These tests support implementation evidence; independent native verification, owner acceptance and epic integration remain separate.

Local validation on 21 November 2026: `npm run verify` completed successfully with 912 backend tests, 62 web tests, web/public builds, lint, product and architecture checks. The pinned Lighthouse strict implementation-plan check also passed. The 24 new domain, API and PostgreSQL tests ran without skips; no browser UI change is part of this child.

Run the focused scenarios with a disposable PostgreSQL database:

```sh
DOMUSMIND_TEST_POSTGRES="<private disposable connection>" dotnet test DomusMind.slnx --filter FullyQualifiedName~AssetObligation
```

The date tests use fixed instants on different household-local days. No server/device fallback timezone is used. Household Settings accepts initial manager configuration; established-zone changes remain outside the slice. Rollback tests refuse to discard obligations/history and preserve the earlier Asset foundation. Exact retries compare the entire first and replayed responses after reload and later cycles.

| Product definition | Executed test methods |
| --- | --- |
| LC-ASSET-OBLIGATION | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.SuspendedRefusesActionsAndReactivationRetainsOverdueCycle`<br>`DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal`<br>`DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.OneTimeResolutionFinishesAndRefusesTerminalActions` |
<!-- pdac:cite id="LC-ASSET-OBLIGATION" digest="sha256:4b2616962208aef948e571c25e92b36d4a74f891640fbcabbdbcf88693717f78" -->
| LC-ASSET-OCCURRENCE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.RecurringCompleteAndSkipEachAdvanceExactlyOnceAndOldCycleIsTerminal`<br>`DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.RescheduleRetainsCycleRuleAndAuditableDates`<br>`DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal` |
<!-- pdac:cite id="LC-ASSET-OCCURRENCE" digest="sha256:a08f423e7a256810872f5748c393abcca547bd9aa1047db124e87e9a97768d9c" -->
| SB-ASSET-OBLIGATION-INITIAL-PENDING | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.InitialPendingAndOverdueDoNotGenerateCycles`<br>`DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.HouseholdTimezoneDeterminesDueAndOverdueWithoutGeneratingMissedCycles` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-INITIAL-PENDING" digest="sha256:e50a2321b6edcfe861a9b5bc7bd3350b2a6d146eef4db8498a080022d326c466" -->
| SB-ASSET-OBLIGATION-SUSPENDED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.SuspendedRefusesActionsAndReactivationRetainsOverdueCycle`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.LifecycleApiPreservesHistoryRejectsSuspendedActionsAndReturnsOriginalRetries` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-SUSPENDED" digest="sha256:f3aa27c7a007796ae2e61e722f53b4c32a96e270f3004b8d535e13dc4d570f45" -->
| SB-ASSET-OBLIGATION-REACTIVATED-OVERDUE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.SuspendedRefusesActionsAndReactivationRetainsOverdueCycle`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.LifecycleApiPreservesHistoryRejectsSuspendedActionsAndReturnsOriginalRetries` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-REACTIVATED-OVERDUE" digest="sha256:ce67c058b7196df900f49daa18555d6e72d7db95d3da79e7dc84bbc3a69a5018" -->
| SB-ASSET-OBLIGATION-CANCELLED-FROM-ACTIVE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-CANCELLED-FROM-ACTIVE" digest="sha256:11cbb4b7e70f68fceb9edd9061276ba9c1a82030f033fc13265f9abbf616fd5d" -->
| SB-ASSET-OBLIGATION-CANCELLED-FROM-SUSPENDED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-CANCELLED-FROM-SUSPENDED" digest="sha256:bf2893493b16bab70b3e86b0e9f29889687afe6191448cfbea5fa8531f03a48e" -->
| SB-ASSET-OCCURRENCE-CANCELLED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-CANCELLED" digest="sha256:828c7d9af4d3670150109c6cc831567b17936e12cd1db45275c0a2048440a1b7" -->
| SB-ASSET-ONE-TIME-COMPLETED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.OneTimeResolutionFinishesAndRefusesTerminalActions`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-ONE-TIME-COMPLETED" digest="sha256:5d122bbd16bb5350752113f68bfdd39f9bccd60f3b3faea78f33a883e9702dc5" -->
| SB-ASSET-ONE-TIME-SKIPPED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.OneTimeResolutionFinishesAndRefusesTerminalActions`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-ONE-TIME-SKIPPED" digest="sha256:dbe6ee87f622bfb89edc89227588ad78b9efa8bd320508d45695e704ed2fc6f7" -->
| SB-ASSET-OCCURRENCE-COMPLETED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.RecurringCompleteAndSkipEachAdvanceExactlyOnceAndOldCycleIsTerminal`<br>`DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ReloadPreservesRescheduleOutcomesAndExactOriginalRetryAcrossLaterCycles` |
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-COMPLETED" digest="sha256:ded69ef148e8d9d978c28d16f2cf02d58ba988c8e6fe3d2e7657f9b876f8a1da" -->
| SB-ASSET-OCCURRENCE-SKIPPED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.RecurringCompleteAndSkipEachAdvanceExactlyOnceAndOldCycleIsTerminal`<br>`DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ReloadPreservesRescheduleOutcomesAndExactOriginalRetryAcrossLaterCycles` |
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-SKIPPED" digest="sha256:27f549f54f4481ea3168b06168f2c0bcbb98f4ebed528a39691f72c0a68dc4d4" -->
| SB-ASSET-OCCURRENCE-RESCHEDULED | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.RescheduleRetainsCycleRuleAndAuditableDates`<br>`DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ReloadPreservesRescheduleOutcomesAndExactOriginalRetryAcrossLaterCycles` |
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-RESCHEDULED" digest="sha256:bb8cfa5672f67bc9ff2bbe26b46409f1229c24f3a2b90fabba4d7b1127b70f19" -->
| SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-COMPLETE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.SuspendedRefusesActionsAndReactivationRetainsOverdueCycle`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.LifecycleApiPreservesHistoryRejectsSuspendedActionsAndReturnsOriginalRetries` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-COMPLETE" digest="sha256:5c4cb2a4ebe48bc285720cd436d1727537e430cdce955f0da72c8b8dd8c8ce7e" -->
| SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-SKIP | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.SuspendedRefusesActionsAndReactivationRetainsOverdueCycle`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.LifecycleApiPreservesHistoryRejectsSuspendedActionsAndReturnsOriginalRetries` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-SKIP" digest="sha256:3edddfe53ef943985188be688d441d8ec3839ec03a9ce149512bd453328cbdd2" -->
| SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-RESCHEDULE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.SuspendedRefusesActionsAndReactivationRetainsOverdueCycle`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.LifecycleApiPreservesHistoryRejectsSuspendedActionsAndReturnsOriginalRetries` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-RESCHEDULE" digest="sha256:42e88bb2aacc937a75af839e39624a479471dc1ebf3117b38e4aebf920af9f87" -->
| SB-ASSET-OBLIGATION-CANCELLED-REFUSES-COMPLETE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-CANCELLED-REFUSES-COMPLETE" digest="sha256:b97ab9ec79b45e1404f5bac901fdcc813ae8251f2cc0cc1c5b9b64f0bf23e1a4" -->
| SB-ASSET-OBLIGATION-CANCELLED-REFUSES-SKIP | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-CANCELLED-REFUSES-SKIP" digest="sha256:a6e75937cb9d49e562b324cbcd39757db59a87c24fe78652ceeb7791653658cc" -->
| SB-ASSET-OBLIGATION-CANCELLED-REFUSES-RESCHEDULE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-CANCELLED-REFUSES-RESCHEDULE" digest="sha256:254823bbbe484cbaa3cfc943eded306a4b77c70763aadf4fc3e15a22f6f405a1" -->
| SB-ASSET-OBLIGATION-FINISHED-REFUSES-COMPLETE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.OneTimeResolutionFinishesAndRefusesTerminalActions`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-FINISHED-REFUSES-COMPLETE" digest="sha256:107c995ba07901e16dd9ca7483f8d07bf731919860790ea161415492ed3228b1" -->
| SB-ASSET-OBLIGATION-FINISHED-REFUSES-SKIP | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.OneTimeResolutionFinishesAndRefusesTerminalActions`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-FINISHED-REFUSES-SKIP" digest="sha256:62b25784120a741fc87138e50d41600cdc745322d5047e4a76579994b967f7ff" -->
| SB-ASSET-OBLIGATION-FINISHED-REFUSES-RESCHEDULE | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.OneTimeResolutionFinishesAndRefusesTerminalActions`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-FINISHED-REFUSES-RESCHEDULE" digest="sha256:5309e0ab0e8dad36feaaa2d526f1e69039da2a1964b54695b6bd27ce94ebd95a" -->
| SB-ASSET-OBLIGATION-CANCELLED-REFUSES-REACTIVATION | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.OneTimeCompletionSkipAndCancellationAreDistinctTerminalOutcomes` |
<!-- pdac:cite id="SB-ASSET-OBLIGATION-CANCELLED-REFUSES-REACTIVATION" digest="sha256:67fd316c5b622aaf29af777a2ad1782202a5363223b1bcdfbc30400279f6c897" -->
| SB-ASSET-OCCURRENCE-RETRY-DOES-NOT-ADVANCE-NEXT | `DomusMind.Domain.Tests.HouseholdAssets.AssetObligationTests.RetryReturnsOriginalResultAcrossFurtherCyclesSuspensionAndArchive`<br>`DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ReloadPreservesRescheduleOutcomesAndExactOriginalRetryAcrossLaterCycles` |
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-RETRY-DOES-NOT-ADVANCE-NEXT" digest="sha256:4bb3f765dcae5574d8287264df52248ef5a5832f13d64faacd39464dd209e6f2" -->
| SB-ASSET-OCCURRENCE-CONCURRENT-OUTCOMES | `DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.CompleteVersusSkipWaitsOnRealLockAndOnlyOneOutcomeWins`<br>`DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.StaleTrackedObligationCannotOverwriteWinningOutcomeOrAppendHistory` |
<!-- pdac:cite id="SB-ASSET-OCCURRENCE-CONCURRENT-OUTCOMES" digest="sha256:eab98e328ad2c2e319b35a31c27f5f4630c147c610d3a2761336ae1ab9317b86" -->
| SB-ASSET-ARCHIVED-ASSET-REFUSES-OBLIGATION-COMPLETION | `DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ArchiveWinsRaceWithoutResolvingCycleAndRestoreRetainsOverdueOrSuspendedState`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.ArchivedAssetRefusesDefinitionsAndMutationsButAllowsConsultationAndAcceptedRetries` |
<!-- pdac:cite id="SB-ASSET-ARCHIVED-ASSET-REFUSES-OBLIGATION-COMPLETION" digest="sha256:cea49878e0e0a9d29ea2dfca55bc0cc06aa46cbe997b88a2e2465a758023088e" -->
| SB-ASSET-RESTORED-ASSET-RETAINS-OBLIGATION-DATE | `DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ArchiveWinsRaceWithoutResolvingCycleAndRestoreRetainsOverdueOrSuspendedState` |
<!-- pdac:cite id="SB-ASSET-RESTORED-ASSET-RETAINS-OBLIGATION-DATE" digest="sha256:f14ca6dc8b4327b2ce26b3f9666afa27ec9b1840cee260da4e43f805ccc24029" -->
| SB-ASSET-RESTORED-ASSET-KEEPS-OBLIGATION-SUSPENDED | `DomusMind.Infrastructure.Tests.HouseholdAssets.AssetObligationPersistenceTests.ArchiveWinsRaceWithoutResolvingCycleAndRestoreRetainsOverdueOrSuspendedState`<br>`DomusMind.Api.Tests.HouseholdAssets.AssetObligationsApiTests.ArchivedAssetRefusesDefinitionsAndMutationsButAllowsConsultationAndAcceptedRetries` |
<!-- pdac:cite id="SB-ASSET-RESTORED-ASSET-KEEPS-OBLIGATION-SUSPENDED" digest="sha256:d04e6cbf1581758daf10d883e88fc8febf18ac2066d87a4fd0ce9a626d4b3d7e" -->
