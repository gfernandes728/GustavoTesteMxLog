using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.DeleteUser;

public record DeleteUserCommand
(
    Guid Id
) : IRequest<bool>;