using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.MealPlanning.GetFamilyRecipes;

public sealed class GetFamilyRecipesQueryHandler : IQueryHandler<GetFamilyRecipesQuery, GetFamilyRecipesResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public GetFamilyRecipesQueryHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<GetFamilyRecipesResponse> Handle(GetFamilyRecipesQuery query, CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            query.RequestedByUserId, query.FamilyId, cancellationToken);

        if (!canAccess)
            throw new InvalidOperationException("Access to this family is denied.");

        var familyId = FamilyId.From(query.FamilyId);

        var recipes = await _dbContext.Set<Recipe>()
            .AsNoTracking()
            .Where(r => r.FamilyId == familyId)
            .OrderBy(r => r.Name)
            .Select(r => new FamilyRecipeItem(
                r.Id.Value,
                r.FamilyId.Value,
                r.Name,
                r.Description,
                r.PrepTimeMinutes,
                r.CookTimeMinutes,
                r.Servings,
                r.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return new GetFamilyRecipesResponse(recipes);
    }
}
