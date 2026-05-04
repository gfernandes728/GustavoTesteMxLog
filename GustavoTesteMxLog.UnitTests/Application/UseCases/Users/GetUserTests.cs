using AutoBogus;
using GustavoTesteMxLog.Application.UseCases.Users.GetUser;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Interfaces;
using Moq;

namespace GustavoTesteMxLog.UnitTests.Application.UseCases.Users;

public class GetUserTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly Mock<IUserRepository> _userRepository = new();

    private readonly GetUserQueryHandler _handler;

    public GetUserTests()
    {
        _handler = new
            (
                _userRepository.Object
            );
    }

    [Fact]
    public async Task GetUser_UserIsNull_Test()
    {
        User? user = null;

        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new GetUserQuery
            (
                Id: _autoFaker.Generate<Guid>()
            );

        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Usuário não existe.", exception.Message);
    }

    [Fact]
    public async Task GetUser_ReturnUser_Test()
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

        var request = new GetUserQuery
            (
                Id: _autoFaker.Generate<Guid>()
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.Equal(user.UpdatedAt, result.UpdatedAt);
    }
}
