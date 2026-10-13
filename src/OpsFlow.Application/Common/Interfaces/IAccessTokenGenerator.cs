using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Common.Interfaces;

public interface IAccessTokenGenerator
{
    string GenerateToken(User user);
}
