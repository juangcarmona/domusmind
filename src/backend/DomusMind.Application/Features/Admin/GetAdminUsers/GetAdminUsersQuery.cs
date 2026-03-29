using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.GetAdminUsers;

public sealed record GetAdminUsersQuery(string? Search = null) : IQuery<AdminUserListResponse>;
