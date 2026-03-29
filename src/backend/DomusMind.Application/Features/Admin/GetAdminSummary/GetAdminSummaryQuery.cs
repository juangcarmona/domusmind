using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.GetAdminSummary;

public sealed record GetAdminSummaryQuery() : IQuery<AdminSummaryResponse>;
