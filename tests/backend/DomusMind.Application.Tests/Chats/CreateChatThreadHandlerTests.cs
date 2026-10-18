using DomusMind.Application.Features.Chats.CreateChatThread;
using DomusMind.Application.Tests.Features.Family;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using DomusMind.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Tests.Chats;

public sealed class CreateChatThreadHandlerTests
{
    [Fact]
    public async Task Handle_HouseholdThread_IncludesEveryHouseholdMember()
    {
        var db = new DomusMindDbContext(new DbContextOptionsBuilder<DomusMindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var familyId = FamilyId.New();
        var userId = Guid.NewGuid();
        var family = Domain.Family.Family.Create(familyId, FamilyName.Create("Test Household"), null, DateTime.UtcNow);
        var creator = family.AddMember(MemberId.New(), MemberName.Create("Alex"), MemberRole.Adult,
            false, null, DateTime.UtcNow, userId);
        var otherMember = family.AddMember(MemberId.New(), MemberName.Create("Sam"), MemberRole.Adult,
            false, null, DateTime.UtcNow);
        db.Set<Domain.Family.Family>().Add(family);
        await db.SaveChangesAsync();

        var handler = new CreateChatThreadHandler(db, new StubEventLogWriter(), new StubFamilyAuthorizationService());
        var result = await handler.Handle(new CreateChatThreadCommand(familyId.Value, "Household", "Household",
            [creator.Id.Value], userId), CancellationToken.None);

        result.ParticipantMemberIds.Should().BeEquivalentTo([creator.Id.Value, otherMember.Id.Value]);
    }
}
