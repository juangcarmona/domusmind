namespace DomusMind.Infrastructure.Persistence.Mcp;

public static class McpWriteApprovalStatuses
{
    public const string Pending = "PENDING";
    public const string Approved = "APPROVED";
    public const string Consumed = "CONSUMED";
    public const string Cancelled = "CANCELLED";
    public const string Expired = "EXPIRED";
}

public static class McpIdempotencyStatuses
{
    public const string PendingApproval = "PENDING_APPROVAL";
    public const string Executing = "EXECUTING";
    public const string Completed = "COMPLETED";
    public const string Cancelled = "CANCELLED";
    public const string Tombstone = "TOMBSTONE";
}

public sealed class McpWriteApprovalRecord
{
    public Guid Id { get; set; }
    public string OperationNamespace { get; set; } = null!;
    public Guid UserId { get; set; }
    public string ClientId { get; set; } = null!;
    public Guid FamilyId { get; set; }
    public Guid ListId { get; set; }
    public string IdempotencyKey { get; set; } = null!;
    public string PayloadHash { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Quantity { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAtUtc { get; set; }
    public DateTime? ConsumedAtUtc { get; set; }
}

public sealed class McpIdempotencyRecord
{
    public Guid Id { get; set; }
    public string OperationNamespace { get; set; } = null!;
    public Guid UserId { get; set; }
    public string ClientId { get; set; } = null!;
    public Guid FamilyId { get; set; }
    public string IdempotencyKey { get; set; } = null!;
    public string PayloadHash { get; set; } = null!;
    public string Status { get; set; } = null!;
    public Guid ListId { get; set; }
    public Guid? ItemId { get; set; }
    public Guid? ApprovalRequestId { get; set; }
    public string? ResultJson { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime? ResultExpiresAtUtc { get; set; }
    public DateTime? TombstoneUntilUtc { get; set; }
}
