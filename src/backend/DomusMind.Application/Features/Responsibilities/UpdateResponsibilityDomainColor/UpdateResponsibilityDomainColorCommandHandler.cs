using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Responsibilities.UpdateResponsibilityDomainColor;

public sealed class UpdateResponsibilityDomainColorCommandHandler
    : ICommandHandler<UpdateResponsibilityDomainColorCommand, UpdateResponsibilityDomainColorResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public UpdateResponsibilityDomainColorCommandHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<UpdateResponsibilityDomainColorResponse> Handle(
        UpdateResponsibilityDomainColorCommand command,
        CancellationToken cancellationToken)
    {
        var domain = await _dbContext
            .Set<ResponsibilityDomain>()
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

        HexColor newColor;
        try
        {
            newColor = HexColor.From(command.Color);
        }
        catch (ArgumentException ex)
        {
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.InvalidInput, ex.Message);
        }

        domain.Repaint(newColor);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateResponsibilityDomainColorResponse(domain.Id.Value, domain.Color.Value);
    }
}
