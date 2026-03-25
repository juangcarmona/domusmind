using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Responsibilities.RemoveSecondaryOwner;

public sealed class RemoveSecondaryOwnerCommandHandler
    : ICommandHandler<RemoveSecondaryOwnerCommand, RemoveSecondaryOwnerResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public RemoveSecondaryOwnerCommandHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<RemoveSecondaryOwnerResponse> Handle(
        RemoveSecondaryOwnerCommand command,
        CancellationToken cancellationToken)
    {
        var domain = await _dbContext
            .Set<Domain.Responsibilities.ResponsibilityDomain>()
            .SingleOrDefaultAsync(
                d => d.Id == ResponsibilityDomainId.From(command.ResponsibilityDomainId),
                cancellationToken);

        if (domain is null)
            throw new ResponsibilitiesException(
                ResponsibilitiesErrorCode.ResponsibilityDomainNotFound,
                "Responsibility domain was not found.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, domain.FamilyId.Value, cancellationToken);

        if (!canAccess)
            throw new ResponsibilitiesException(
                ResponsibilitiesErrorCode.AccessDenied, "Access to this family is denied.");

        domain.RemoveSecondaryOwner(MemberId.From(command.MemberId));

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RemoveSecondaryOwnerResponse(domain.Id.Value, command.MemberId);
    }
}
