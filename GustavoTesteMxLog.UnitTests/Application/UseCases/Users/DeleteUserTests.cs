using AutoBogus;
using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Application.UseCases.Users.DeleteUser;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Interfaces;
using Moq;

namespace GustavoTesteMxLog.UnitTests.Application.UseCases.Users;

public class DeleteUserTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ICurrentUserService> _currentUserService = new();

    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserTests()
    {
        _handler = new
            (
                _userRepository.Object,
                _currentUserService.Object
            );
    }

    [Fact]
    public async Task DeleteUser_UserIsNull_Test()
    {
        User? user = null;

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new DeleteUserCommand
            (
                Id: _autoFaker.Generate<Guid>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Usuário não existe.", exception.Message);
    }

    [Fact]
    public async Task DeleteUser_UserDefaultIsNotDeleted_Test()
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

        var request = new DeleteUserCommand
            (
                Id: _autoFaker.Generate<Guid>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal($"Usuário principal \"{user.Email}\" não pode ser excluído.", exception.Message);
    }

    [Fact]
    public async Task DeleteUser_UserLoggedNotDeleted_Test()
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

        _currentUserService
            .Setup(x => x.UserId)
            .Returns(user.Id);

        var request = new DeleteUserCommand
            (
                Id: _autoFaker.Generate<Guid>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Usuário logado não pode ser excluído.", exception.Message);
    }

    [Fact]
    public async Task DeleteUser_DeletedUser_Test()
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

        var currentUserId = _autoFaker.Generate<Guid>();
        while (currentUserId == user.Id) currentUserId = _autoFaker.Generate<Guid>();

        _currentUserService
            .Setup(x => x.UserId)
            .Returns(currentUserId);

        var isDeleted = _autoFaker.Generate<bool>();

        _userRepository
            .Setup(x => x.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(isDeleted);

        var request = new DeleteUserCommand
            (
                Id: _autoFaker.Generate<Guid>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(isDeleted, result);
    }
}
