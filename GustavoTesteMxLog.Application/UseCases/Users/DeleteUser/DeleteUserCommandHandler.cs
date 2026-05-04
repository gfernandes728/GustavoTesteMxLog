using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Infra.Interfaces;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.DeleteUser;

public class DeleteUserCommandHandler
(
    IUserRepository userRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception("Usuário não existe.");

        if (user.Email == "admin@mxlog.com.br")
            throw new Exception($"Usuário principal \"{user.Email}\" não pode ser excluído.");

        if (_currentUserService.UserId == user.Id)
            throw new Exception("Usuário logado não pode ser excluído.");

        return await _userRepository.DeleteAsync(user, cancellationToken);
    }
}