using GustavoTesteMxLog.Domain.Responses;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.GetDashboard;

public record GetDashboardQuery
(
    string? Search,
    int Page,
    int PageSize
) : IRequest<PaginatedResponse<UserResponse>>;
