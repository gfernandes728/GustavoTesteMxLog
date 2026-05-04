using AutoBogus;
using GustavoTesteMxLog.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GustavoTesteMxLog.UnitTests.Domain;

public class RequestTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    [Fact]
    public void DashboardRequest_Test()
    {
        var search = _autoFaker.Generate<string>();
        var page = _autoFaker.Generate<int>();
        var pageSize = _autoFaker.Generate<int>();

        var request = new DashboardRequest
        {
            Search = search,
            Page = page,
            PageSize = pageSize
        };

        Assert.NotNull(request);
        Assert.Equal(search, request.Search);
        Assert.Equal(page, request.Page);
        Assert.Equal(pageSize, request.PageSize);
    }

    [Fact]
    public void LoginRequest_Test()
    {
        var email = _autoFaker.Generate<string>();
        var password = _autoFaker.Generate<string>();

        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        Assert.NotNull(request);
        Assert.Equal(email, request.Email);
        Assert.Equal(password, request.Password);
    }

    [Fact]
    public void UserRequest_Test()
    {
        var name = _autoFaker.Generate<string>();
        var email = _autoFaker.Generate<string>();
        var password = _autoFaker.Generate<string>();

        var request = new UserRequest
        {
            Name = name,
            Email = email,
            Password = password
        };

        Assert.NotNull(request);
        Assert.Equal(name, request.Name);
        Assert.Equal(email, request.Email);
        Assert.Equal(password, request.Password);
    }
}
