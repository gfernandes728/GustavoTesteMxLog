using AutoBogus;
using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Application.UseCases.Auth.Login;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Interfaces;
using Moq;

namespace GustavoTesteMxLog.UnitTests.Application.UseCases.Auth;

public class LoginTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IJwtTokenService> _jwtTokenService = new();

    private readonly LoginQueryHandler _handler;

    public LoginTests()
    {
        _handler = new
            (
                _userRepository.Object,
                _passwordHasher.Object,
                _jwtTokenService.Object
            );
    }

    [Fact]
    public async Task Login_UserIsNull_Test()
    {
        User? user = null;

        _userRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new LoginQuery
            (
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_PasswordIsNotVerify_Test()
    {
        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: _autoFaker.Generate<string>(),
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var passwordResult = false;

        _passwordHasher
            .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(passwordResult);

        var request = new LoginQuery
            (
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_TokenGenerate_Test()
    {
        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: _autoFaker.Generate<string>(),
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var passwordResult = true;

        _passwordHasher
            .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(passwordResult);

        var token = _autoFaker.Generate<string>();

        _jwtTokenService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(token);

        var request = new LoginQuery
            (
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(token, result);
    }
}
