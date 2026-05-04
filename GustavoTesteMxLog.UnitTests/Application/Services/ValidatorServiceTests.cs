using GustavoTesteMxLog.Application.Services;

namespace GustavoTesteMxLog.UnitTests.Application.Services;

public class ValidatorServiceTests
{
    private readonly ValidatorService _validatorService = new();

    [Theory]
    [InlineData("teste@email.com", true)]
    [InlineData("user.name@domain.com", true)]
    [InlineData("user@domain", false)]
    [InlineData("user@.com", false)]
    [InlineData("user@", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", false)]
    public void ValidatorService_IsEmailValid_Tests(string email, bool expected)
    {
        var result = _validatorService.IsEmailValid(email);
        Assert.Equal(expected, result);
    }
}
