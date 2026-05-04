using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Domain.Responses;
using GustavoTesteMxLog.Infra.Interfaces;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.CreateUser;

public class CreateUserCommandHandler
(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IValidatorService validatorService
) : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidatorService _validatorService = validatorService;

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new Exception("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new Exception("Senha é obrigatória.");

        if (!_validatorService.IsEmailValid(request.Email))
            throw new Exception("Email inválido.");

        if (await _userRepository.ExistsUserByEmailAsync(request.Email, cancellationToken))
            throw new Exception("Email já cadastrado, para outro Usuário.");

        var user = User.TryCreate
        (
            name: request.Name,
            email: request.Email,
            password: _passwordHasher.Hash(request.Password)
        );

        var data = await _userRepository.AddAsync(user, cancellationToken);

        return new UserResponse
            (
                id: data.Id,
                name: data.Name,
                email: data.Email,
                createdAt: data.CreatedAt
            );
    }
}