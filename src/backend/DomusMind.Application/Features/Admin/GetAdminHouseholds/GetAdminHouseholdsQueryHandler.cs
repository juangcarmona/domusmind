using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Contracts.Admin;
using Microsoft.EntityFrameworkCore;
using FamilyAggregate = DomusMind.Domain.Family.Family;

namespace DomusMind.Application.Features.Admin.GetAdminHouseholds;

public sealed class GetAdminHouseholdsQueryHandler
    : IQueryHandler<GetAdminHouseholdsQuery, AdminHouseholdListResponse>
{
    private readonly IDomusMindDbContext _db;

    public GetAdminHouseholdsQueryHandler(IDomusMindDbContext db)
    {
        _db = db;
    }

    public async Task<AdminHouseholdListResponse> Handle(
        GetAdminHouseholdsQuery query,
        CancellationToken cancellationToken)
    {
        var families = await _db.Set<FamilyAggregate>()
            .AsNoTracking()
            .Include(f => f.Members)
            .ToListAsync(cancellationToken);

        IEnumerable<FamilyAggregate> filtered = families;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLower();
            filtered = families.Where(f => f.Name.Value.ToLower().Contains(term));
        }

        var items = filtered
            .OrderByDescending(f => f.CreatedAtUtc)
            .Select(f => new AdminHouseholdSummary(f.Id.Value, f.Name.Value, f.CreatedAtUtc, f.Members.Count))
            .ToList();

        return new AdminHouseholdListResponse(items);
    }
}
