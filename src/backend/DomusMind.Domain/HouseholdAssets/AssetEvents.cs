using DomusMind.Domain.Abstractions;

namespace DomusMind.Domain.HouseholdAssets;

public sealed record AssetRegistered(Guid AssetId, Guid FamilyId, DateTime OccurredAtUtc) : IDomainEvent;
public sealed record AssetUpdated(Guid AssetId, Guid FamilyId, DateTime OccurredAtUtc) : IDomainEvent;
public sealed record AssetArchived(Guid AssetId, Guid FamilyId, DateTime OccurredAtUtc) : IDomainEvent;
public sealed record AssetRestored(Guid AssetId, Guid FamilyId, DateTime OccurredAtUtc) : IDomainEvent;
public sealed record AssetLocationRegistered(Guid LocationId, Guid FamilyId, DateTime OccurredAtUtc) : IDomainEvent;
