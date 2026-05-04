using GustavoTesteMxLog.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GustavoTesteMxLog.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword(new object(), password);

    public bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword(new object(), hash, password) == PasswordVerificationResult.Success;
}
