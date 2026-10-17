using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Shared;
using DomusMind.Domain.Tasks;
using DomusMind.Domain.Tasks.Enums;
using DomusMind.Domain.Tasks.ValueObjects;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SystemDayOfWeek = System.DayOfWeek;

namespace DomusMind.Infrastructure.Tests.Persistence;

public sealed class CollectionValueComparerTests
{
    [Fact]
    public async Task RecipeCollectionUpdatesAreDetectedAndPersisted()
    {
        await using var db = CreateDbContext();
        await db.Database.EnsureCreatedAsync();

        var recipe = Recipe.Create(
            RecipeId.New(),
            FamilyId.New(),
            "Pasta",
            null,
            null,
            null,
            null,
            false,
            [MealType.Dinner],
            ["pasta"],
            DateTime.UtcNow);
        db.Add(recipe);
        await db.SaveChangesAsync();

        var originalUpdatedAt = recipe.UpdatedAtUtc;
        recipe.Update(
            recipe.Name,
            recipe.Description,
            recipe.PrepTimeMinutes,
            recipe.CookTimeMinutes,
            recipe.Servings,
            recipe.IsFavorite,
            [MealType.Lunch],
            ["quick"]);
        db.Entry(recipe).Property(x => x.UpdatedAtUtc).CurrentValue = originalUpdatedAt;

        db.ChangeTracker.DetectChanges();
        db.Entry(recipe).Property(x => x.AllowedMealTypes).IsModified.Should().BeTrue();
        db.Entry(recipe).Property(x => x.Tags).IsModified.Should().BeTrue();

        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var saved = await db.Set<Recipe>().SingleAsync(x => x.Id == recipe.Id);
        saved.AllowedMealTypes.Should().Equal(MealType.Lunch);
        saved.Tags.Should().Equal("quick");
    }

    [Fact]
    public async Task RoutineDaysOfWeekMutationIsDetectedAndPersisted()
    {
        await using var db = CreateDbContext();
        await db.Database.EnsureCreatedAsync();

        var routine = CreateRoutine(RoutineSchedule.Weekly([SystemDayOfWeek.Monday]));
        db.Add(routine);
        await db.SaveChangesAsync();

        ((SystemDayOfWeek[])routine.Schedule.DaysOfWeek)[0] = SystemDayOfWeek.Wednesday;
        db.ChangeTracker.DetectChanges();

        db.Entry(routine.Schedule).Property(x => x.DaysOfWeek).IsModified.Should().BeTrue();
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var saved = await db.Routines.SingleAsync(x => x.Id == routine.Id);
        saved.Schedule.DaysOfWeek.Should().Equal(SystemDayOfWeek.Wednesday);
    }

    [Fact]
    public async Task RoutineDaysOfMonthMutationIsDetectedAndPersisted()
    {
        await using var db = CreateDbContext();
        await db.Database.EnsureCreatedAsync();

        var routine = CreateRoutine(RoutineSchedule.Monthly([5]));
        db.Add(routine);
        await db.SaveChangesAsync();

        ((int[])routine.Schedule.DaysOfMonth)[0] = 12;
        db.ChangeTracker.DetectChanges();

        db.Entry(routine.Schedule).Property(x => x.DaysOfMonth).IsModified.Should().BeTrue();
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var saved = await db.Routines.SingleAsync(x => x.Id == routine.Id);
        saved.Schedule.DaysOfMonth.Should().Equal(12);
    }

    private static DomusMindDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Throw(CoreEventId.CollectionWithoutComparer))
            .Options;

        return new DomusMindDbContext(options);
    }

    private static Routine CreateRoutine(RoutineSchedule schedule)
        => Routine.Create(
            RoutineId.New(),
            FamilyId.New(),
            RoutineName.Create("Test routine"),
            RoutineScope.Household,
            RoutineKind.Scheduled,
            HexColor.From("#336699"),
            schedule,
            Array.Empty<MemberId>());
}
