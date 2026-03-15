using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Responsibilities.AssignSecondaryOwner;

public sealed class AssignSecondaryOwnerCommandHandler
    : ICommandHandler<AssignSecondaryOwnerCommand, AssignSecondaryOwnerResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public AssignSecondaryOwnerCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<AssignSecondaryOwnerResponse> Handle(
        AssignSecondaryOwnerCommand command,
        CancellationToken cancellationToken)
    {
        var responsibilityDomain = await _dbContext
            .Set<Domain.Responsibilities.ResponsibilityDomain>()
            .SingleOrDefaultAsync(
                d => d.Id == ResponsibilityDomainId.From(command.ResponsibilityDomainId),
                cancellationToken);

        if (responsibilityDomain is null)
            throw new ResponsibilitiesException(
                ResponsibilitiesErrorCode.ResponsibilityDomainNotFound,
                "Responsibility domain was not found.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, responsibilityDomain.FamilyId.Value, cancellationToken);

        if (!canAccess)
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.AccessDenied, "Access to this family is denied.");

        try
        {
            responsibilityDomain.AssignSecondaryOwner(MemberId.From(command.MemberId));
        }
        catch (InvalidOperationException ex)
        {
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.DuplicateSecondaryOwner, ex.Message);
        }

        await _eventLogWriter.WriteAsync(responsibilityDomain.DomainEvents, cancellationToken);
        responsibilityDomain.ClearDomainEvents();

        return new AssignSecondaryOwnerResponse(command.ResponsibilityDomainId, command.MemberId);
    }
}
