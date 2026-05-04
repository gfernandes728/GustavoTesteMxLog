using GustavoTesteMxLog.Domain.Responses;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.UpdateUser;

public record UpdateUserCommand
(
    Guid Id,
    string Name,
    string Email,
    string? Password
) : IRequest<UserResponse>;
