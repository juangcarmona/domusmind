using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Responsibilities;
using DomusMind.Contracts.Responsibilities;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.ValueObjects;
using DomusMind.Domain.Shared;

namespace DomusMind.Application.Features.Responsibilities.CreateResponsibilityDomain;

public sealed class CreateResponsibilityDomainCommandHandler
    : ICommandHandler<CreateResponsibilityDomainCommand, CreateResponsibilityDomainResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;

    public CreateResponsibilityDomainCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
    }

    public async Task<CreateResponsibilityDomainResponse> Handle(
        CreateResponsibilityDomainCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.InvalidInput, "Responsibility domain name is required.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, command.FamilyId, cancellationToken);

        if (!canAccess)
            throw new ResponsibilitiesException(ResponsibilitiesErrorCode.AccessDenied, "Access to this family is denied.");

        var id = ResponsibilityDomainId.New();
        var familyId = FamilyId.From(command.FamilyId);
        var name = ResponsibilityAreaName.Create(command.Name);
        var color = HexColor.From("#6A4C93");
        var now = DateTime.UtcNow;

        var domain = Domain.Responsibilities.ResponsibilityDomain.Create(id, familyId, name, color, now);

        _dbContext.Set<Domain.Responsibilities.ResponsibilityDomain>().Add(domain);

        await _eventLogWriter.WriteAsync(domain.DomainEvents, cancellationToken);
        domain.ClearDomainEvents();

        return new CreateResponsibilityDomainResponse(id.Value, familyId.Value, name.Value, color.Value, now);
    }
}
