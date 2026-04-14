using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.MealPlanning;

namespace DomusMind.Application.Features.MealPlanning.GetFamilyRecipes;

public sealed record GetFamilyRecipesQuery(
    Guid FamilyId,
    Guid RequestedByUserId) : IQuery<GetFamilyRecipesResponse>;
