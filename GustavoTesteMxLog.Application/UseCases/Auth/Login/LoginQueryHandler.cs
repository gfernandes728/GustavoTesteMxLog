using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Infra.Interfaces;
using MediatR;

namespace GustavoTesteMxLog.Application.UseCases.Auth.Login;

public class LoginQueryHandler
(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService
) : IRequestHandler<LoginQuery, string?>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task<string?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return null;

        if (!_passwordHasher.Verify(user.Password, request.Password))
            return null;

        return _jwtTokenService.GenerateToken(user);
    }
}
