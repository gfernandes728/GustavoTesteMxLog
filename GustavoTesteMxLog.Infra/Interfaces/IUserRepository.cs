using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Aggregates;

namespace GustavoTesteMxLog.Infra.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsUserByEmailAsync(Guid id, string email, CancellationToken cancellationToken = default);

    Task<PaginatedResult<User>> GetPaginatedAsync
    (
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
}
