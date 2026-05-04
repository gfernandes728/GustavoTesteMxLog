using AutoBogus;
using GustavoTesteMxLog.Application.Services;

namespace GustavoTesteMxLog.UnitTests.Application.Services;

public class PasswordHasherTests
{
    private readonly IAutoFaker _autoFaker = AutoFaker.Create();

    private readonly PasswordHasher _passwordHasher = new();

    [Fact]
    public void PasswordHasher_Hash_ReturnHashedPassword_Test()
    {
        var password = _autoFaker.Generate<string>();
        var hash = _passwordHasher.Hash(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void PasswordHasher_Verify_PasswordIsCorrect_Test()
    {
        var password = _autoFaker.Generate<string>();
        var hash = _passwordHasher.Hash(password);

        var result = _passwordHasher.Verify(hash, password);

        Assert.True(result);
    }

    [Fact]
    public void PasswordHasher_Verify_PasswordIsIncorrect_Test()
    {
        var password = _autoFaker.Generate<string>();

        var wrongPassword = _autoFaker.Generate<string>();
        while (password == wrongPassword) wrongPassword = _autoFaker.Generate<string>();

        var hash = _passwordHasher.Hash(password);

        var result = _passwordHasher.Verify(hash, wrongPassword);

        Assert.False(result);
    }

    [Fact]
    public void PasswordHasher_Hash_GenerateDifferentHashes_Tests()
    {
        var password = _autoFaker.Generate<string>();

        var hash1 = _passwordHasher.Hash(password);
        var hash2 = _passwordHasher.Hash(password);

        Assert.NotEqual(hash1, hash2);
    }
}
