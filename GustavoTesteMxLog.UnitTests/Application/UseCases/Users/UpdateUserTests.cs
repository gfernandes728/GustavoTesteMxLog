using AutoBogus;
using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Application.UseCases.Users.CreateUser;
using GustavoTesteMxLog.Application.UseCases.Users.GetUser;
using GustavoTesteMxLog.Application.UseCases.Users.UpdateUser;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Interfaces;
using Moq;

namespace GustavoTesteMxLog.UnitTests.Application.UseCases.Users;

public class UpdateUserTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IValidatorService> _validatorService = new();

    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserTests()
    {
        _handler = new
            (
                _userRepository.Object,
                _passwordHasher.Object,
                _validatorService.Object
            );
    }

    [Fact]
    public async Task UpdateUser_UserIsNull_Test()
    {
        User? user = null;

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: _autoFaker.Generate<string>(),
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Usuário não existe.", exception.Message);
    }

    [Fact]
    public async Task UpdateUser_UserDefaultIsNotUpdated_Test()
    {
        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: "admin@mxlog.com.br",
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: _autoFaker.Generate<string>(),
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal($"Usuário principal \"{user.Email}\" não pode ser alterado.", exception.Message);
    }

    [Fact]
    public async Task UpdateUser_NameIsNull_Test()
    {
        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: _autoFaker.Generate<string>(),
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: "",
                Email: _autoFaker.Generate<string>(),
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Nome é obrigatório.", exception.Message);
    }

    [Fact]
    public async Task UpdateUser_EmailInvalid_Test()
    {
        var email = _autoFaker.Generate<string>();

        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: email,
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var isEmailValid = false;

        _validatorService
            .Setup(x => x.IsEmailValid(It.IsAny<string>()))
            .Returns(isEmailValid);

        var requestEmail = _autoFaker.Generate<string>();
        while(email == requestEmail) requestEmail = _autoFaker.Generate<string>();

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: _autoFaker.Generate<string>(),
                Email: requestEmail,
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Email inválido.", exception.Message);
    }

    [Fact]
    public async Task UpdateUser_UserExists_Test()
    {
        var email = _autoFaker.Generate<string>();

        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: email,
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var userExists = true;

        _userRepository
            .Setup(x => x.ExistsUserByEmailAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userExists);

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: _autoFaker.Generate<string>(),
                Email: email,
                Password: _autoFaker.Generate<string>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Email já cadastrado, para outro Usuário.", exception.Message);
    }

    [Fact]
    public async Task UpdateUser_UpdateUserWithoutPassword_Test()
    {
        var email = _autoFaker.Generate<string>();

        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: email,
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var userExists = false;

        _userRepository
            .Setup(x => x.ExistsUserByEmailAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userExists);

        _userRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: _autoFaker.Generate<string>(),
                Email: email,
                Password: null
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task UpdateUser_UpdateUserWithPassword_Test()
    {
        var email = _autoFaker.Generate<string>();

        var user = User.TryCreate
            (
                name: _autoFaker.Generate<string>(),
                email: email,
                password: _autoFaker.Generate<string>()
            );

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var userExists = false;

        _userRepository
            .Setup(x => x.ExistsUserByEmailAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userExists);

        var password = _autoFaker.Generate<string>();

        _passwordHasher
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns(password);

        _userRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

        var request = new UpdateUserCommand
            (
                Id: _autoFaker.Generate<Guid>(),
                Name: _autoFaker.Generate<string>(),
                Email: email,
                Password: _autoFaker.Generate<string>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.NotNull(result.UpdatedAt);
    }
}
