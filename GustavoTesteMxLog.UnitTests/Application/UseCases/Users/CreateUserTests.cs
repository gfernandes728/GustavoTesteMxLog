using AutoBogus;
using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Application.UseCases.Users.CreateUser;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Interfaces;
using Moq;

namespace GustavoTesteMxLog.UnitTests.Application.UseCases.Users;

public class CreateUserTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IValidatorService> _validatorService = new();

    private readonly CreateUserCommandHandler _handler;

    public CreateUserTests()
    {
        _handler = new
            (
                _userRepository.Object,
                _passwordHasher.Object,
                _validatorService.Object
            );
    }

    [Fact]
    public async Task CreateUser_NameIsNull_Test()
    {
        var request = new CreateUserCommand
            (
                Name: "",
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Nome é obrigatório.", exception.Message);
    }

    [Fact]
    public async Task CreateUser_PasswordIsNull_Test()
    {
        var request = new CreateUserCommand
            (
                Name: _autoFaker.Generate<string>(),
                Email: _autoFaker.Generate<string>(),
                Password: null
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Senha é obrigatória.", exception.Message);
    }

    [Fact]
    public async Task CreateUser_EmailInvalid_Test()
    {
        var isEmailValid = false;

        _validatorService
            .Setup(x => x.IsEmailValid(It.IsAny<string>()))
            .Returns(isEmailValid);

        var request = new CreateUserCommand
            (
                Name: _autoFaker.Generate<string>(),
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Email inválido.", exception.Message);
    }

    [Fact]
    public async Task CreateUser_UserExists_Test()
    {
        var isEmailValid = true;

        _validatorService
            .Setup(x => x.IsEmailValid(It.IsAny<string>()))
            .Returns(isEmailValid);

        var userExists = true;

        _userRepository
            .Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userExists);

        var request = new CreateUserCommand
            (
                Name: _autoFaker.Generate<string>(),
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Email já cadastrado, para outro Usuário.", exception.Message);
    }

    [Fact]
    public async Task CreateUser_CreatedUser_Test()
    {
        var isEmailValid = true;

        _validatorService
            .Setup(x => x.IsEmailValid(It.IsAny<string>()))
            .Returns(isEmailValid);

        var userExists = false;

        _userRepository
            .Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userExists);

        var password = _autoFaker.Generate<string>();

        _passwordHasher
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns(password);

        var user = User.TryCreate
        (
            name: _autoFaker.Generate<string>(),
            email: _autoFaker.Generate<string>(),
            password: _autoFaker.Generate<string>()
        );

        _userRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new CreateUserCommand
            (
                Name: _autoFaker.Generate<string>(),
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.Null(result.UpdatedAt);
    }
}
