using GustavoTesteMxLog.Domain.Responses;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.CreateUser;

public record CreateUserCommand
(
    string Name, 
    string Email,
    string? Password
) : IRequest<UserResponse>;
