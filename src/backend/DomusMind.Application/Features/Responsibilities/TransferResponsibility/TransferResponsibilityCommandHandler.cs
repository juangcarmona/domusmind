using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Responsibilities.TransferResponsibility;

public sealed class TransferResponsibilityCommandHandler
    : ICommandHandler<TransferResponsibilityCommand, TransferResponsibilityResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public TransferResponsibilityCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<TransferResponsibilityResponse> Handle(
        TransferResponsibilityCommand command,
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

        responsibilityDomain.TransferPrimaryOwner(MemberId.From(command.NewPrimaryOwnerId));

        await _eventLogWriter.WriteAsync(responsibilityDomain.DomainEvents, cancellationToken);
        responsibilityDomain.ClearDomainEvents();

        return new TransferResponsibilityResponse(command.ResponsibilityDomainId, command.NewPrimaryOwnerId);
    }
}
