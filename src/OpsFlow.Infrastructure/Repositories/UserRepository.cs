using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Users;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Persistence;

namespace OpsFlow.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly OpsFlowDbContext _dbContext;

    public UserRepository(OpsFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AnyAsync(
                user => user.Email == email,
                cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(
            user,
            cancellationToken);
    }
}
