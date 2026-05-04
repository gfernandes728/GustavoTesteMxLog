using GustavoTesteMxLog.Domain.Entities;
using GustavoTesteMxLog.Infra.Aggregates;
using GustavoTesteMxLog.Infra.Data;
using GustavoTesteMxLog.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace GustavoTesteMxLog.Infra.Repositories;

[ExcludeFromCodeCoverage]
public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> ExistsUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return user is not null;
    }

    public async Task<bool> ExistsUserByEmailAsync(Guid id, string email, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id != id && u.Email == email, cancellationToken);
        return user is not null;
    }

    public async Task<PaginatedResult<User>> GetPaginatedAsync
    (
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.Name.Contains(search) || u.Email.Contains(search));

        var total = await query.CountAsync(cancellationToken);

        var data = await query
            .OrderBy(u => u.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<User>
        {
            Data = data,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}