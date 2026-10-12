using Microsoft.AspNetCore.Identity;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Security;

public sealed class AspNetCorePasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(
        User user,
        string password)
    {
        return _passwordHasher.HashPassword(
            user,
            password);
    }

    public bool VerifyPassword(
        User user,
        string passwordHash,
        string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            passwordHash,
            password);

        return result is
            PasswordVerificationResult.Success or
            PasswordVerificationResult.SuccessRehashNeeded;
    }
}
