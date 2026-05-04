using GustavoTesteMxLog.Domain.Responses;
using GustavoTesteMxLog.Infra.Interfaces;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.GetDashboard;

public class GetDashboardQueryHandler
(
    IUserRepository userRepository
) : IRequestHandler<GetDashboardQuery, PaginatedResponse<UserResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<PaginatedResponse<UserResponse>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var data = await _userRepository.GetPaginatedAsync
            (
                search: request.Search,
                page: request.Page <= 0 ? 1 : request.Page,
                pageSize: request.PageSize <= 0 ? 10 : request.PageSize,
                cancellationToken: cancellationToken
            );

        var users = data.Data.Select
                (
                    x => new UserResponse
                    (
                        id: x.Id,
                        name: x.Name,
                        email: x.Email,
                        createdAt: x.CreatedAt,
                        updatedAt: x.UpdatedAt
                    )
                ).ToList();

        return new PaginatedResponse<UserResponse>
            (
                data: users,
                total: data.Total,
                page: data.Page,
                pageSize: data.PageSize
            );
    }
}
