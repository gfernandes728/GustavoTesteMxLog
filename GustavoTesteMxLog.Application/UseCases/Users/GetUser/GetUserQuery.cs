using GustavoTesteMxLog.Domain.Responses;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Users.GetUser;

public record GetUserQuery
(
    Guid Id
) : IRequest<UserResponse>;
