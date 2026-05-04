using AutoBogus;
using GustavoTesteMxLog.Application.UseCases.Users.GetDashboard;
using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Aggregates;
using GustavoTesteMxLog.Infra.Interfaces;
using Moq;

namespace GustavoTesteMxLog.UnitTests.Application.UseCases.Users;

public class GetDashboardTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly Mock<IUserRepository> _userRepository = new();

    private readonly GetDashboardQueryHandler _handler;

    public GetDashboardTests()
    {
        _handler = new
            (
                _userRepository.Object
            );
    }

    [Fact]
    public async Task GetDashboard_PaginatedDataDefault_Test()
    {
        var user = User.TryCreate
        (
            name: _autoFaker.Generate<string>(),
            email: _autoFaker.Generate<string>(),
            password: _autoFaker.Generate<string>()
        );

        var paginated = new PaginatedResult<User>()
        {
            Data = [user],
            Total = 1,
            Page = 1,
            PageSize = 10
        };

        _userRepository
            .Setup(x => x.GetPaginatedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginated);

        var request = new GetDashboardQuery
            (
                Search: _autoFaker.Generate<string>(),
                Page: 0,
                PageSize: 0
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(paginated.Total, result.Total);
        Assert.Equal(paginated.Page, result.Page);
        Assert.Equal(paginated.PageSize, result.PageSize);

        Assert.NotNull(paginated.Data);
        Assert.Equal(paginated.Data.Count(), result.Data.Count);

        Assert.Equal(user.Id, result.Data[0].Id);
        Assert.Equal(user.Name, result.Data[0].Name);
        Assert.Equal(user.Email, result.Data[0].Email);
        Assert.Equal(user.CreatedAt, result.Data[0].CreatedAt);
        Assert.Equal(user.UpdatedAt, result.Data[0].UpdatedAt);
    }

    [Fact]
    public async Task GetDashboard_PaginatedData_Test()
    {
        var user = User.TryCreate
        (
            name: _autoFaker.Generate<string>(),
            email: _autoFaker.Generate<string>(),
            password: _autoFaker.Generate<string>()
        );

        var paginated = new PaginatedResult<User>()
        {
            Data = [user],
            Total = 1,
            Page = 1,
            PageSize = 10
        };

        _userRepository
            .Setup(x => x.GetPaginatedAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginated);

        var request = new GetDashboardQuery
            (
                Search: _autoFaker.Generate<string>(),
                Page: 1,
                PageSize: 10
            );

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(paginated.Total, result.Total);
        Assert.Equal(paginated.Page, result.Page);
        Assert.Equal(paginated.PageSize, result.PageSize);

        Assert.NotNull(paginated.Data);
        Assert.Equal(paginated.Data.Count(), result.Data.Count);

        Assert.Equal(user.Id, result.Data[0].Id);
        Assert.Equal(user.Name, result.Data[0].Name);
        Assert.Equal(user.Email, result.Data[0].Email);
        Assert.Equal(user.CreatedAt, result.Data[0].CreatedAt);
        Assert.Equal(user.UpdatedAt, result.Data[0].UpdatedAt);
    }
}
