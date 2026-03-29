using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.GetAdminHouseholds;

public sealed record GetAdminHouseholdsQuery(string? Search = null) : IQuery<AdminHouseholdListResponse>;
