using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Auth.Login;

public record LoginQuery
(
    string Email,
    string Password
) : IRequest<string?>;
