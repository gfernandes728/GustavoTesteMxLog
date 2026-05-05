using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Domain.Responses;
using GustavoTesteMxLog.Infra.Interfaces;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.UpdateUser;

public class UpdateUserCommandHandler
(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IValidatorService validatorService
) : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidatorService _validatorService = validatorService;

    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception("Usuário não existe.");

        if (user.Email == "admin@mxlog.com.br" && user.Email != request.Email)
            throw new Exception($"Usuário principal \"{user.Email}\" não pode ser alterado.");

        if (request.Name != user.Name && string.IsNullOrWhiteSpace(request.Name))
            throw new Exception("Nome é obrigatório.");

        if (request.Email != user.Email && !_validatorService.IsEmailValid(request.Email))
            throw new Exception("Email inválido.");

        if (await _userRepository.ExistsUserByEmailAsync(request.Id, request.Email, cancellationToken))
            throw new Exception("Email já cadastrado, para outro Usuário.");

        var data = User.TryUpdate
        (
            user: user,
            name: request.Name,
            email: request.Email,
            password: string.IsNullOrWhiteSpace(request.Password) ? user.Password : _passwordHasher.Hash(request.Password)
        );

        await _userRepository.UpdateAsync(data, cancellationToken);

        return new UserResponse
            (
                id: data.Id,
                name: data.Name,
                email: data.Email,
                createdAt: data.CreatedAt,
                updatedAt: data.UpdatedAt
            );
    }
}