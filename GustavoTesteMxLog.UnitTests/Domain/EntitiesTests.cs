using GustavoTesteMxLog.Domain.Entities;

namespace GustavoTesteMxLog.UnitTests.Domain;

public class EntitiesTests
{
    [Fact]
    public void User_Test()
    {
        var user = new User();
        Assert.IsType<User>(user);
    }
}
