using GustavoTesteMxLog.Domain.Responses;
using GustavoTesteMxLog.Infra.Interfaces;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.GetUser;

public class GetUserQueryHandler
(
    IUserRepository userRepository
) : IRequestHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception("Usuário não existe.");

        return new UserResponse
            (
                id: user.Id,
                name: user.Name,
                email: user.Email,
                createdAt: user.CreatedAt,
                updatedAt: user.UpdatedAt
            );
    }
}
