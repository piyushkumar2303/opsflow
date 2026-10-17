using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Common.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(
        User user,
        string password);

    bool VerifyPassword(
        User user,
        string passwordHash,
        string password);
}
