using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Responsibilities.RenameResponsibilityDomain;

public sealed class RenameResponsibilityDomainCommandHandler
    : ICommandHandler<RenameResponsibilityDomainCommand, RenameResponsibilityDomainResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public RenameResponsibilityDomainCommandHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<RenameResponsibilityDomainResponse> Handle(
        RenameResponsibilityDomainCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ResponsibilitiesException(
                ResponsibilitiesErrorCode.InvalidInput, "Area name is required.");

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

        ResponsibilityAreaName newName;
        try
        {
            newName = ResponsibilityAreaName.Create(command.Name);
        }
        catch (ArgumentException ex)
        {
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.InvalidInput, ex.Message);
        }

        domain.Rename(newName);

        return new RenameResponsibilityDomainResponse(domain.Id.Value, domain.Name.Value);
    }
}
