using Microsoft.EntityFrameworkCore;
using StudioBookingManagement.Domain.Entities;
using StudioBookingManagement.Domain.Repositories;
using StudioBookingManagement.Domain.ValueObjects;
using StudioBookingManagement.Infrastructure.Data;

namespace StudioBookingManagement.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(StudioBookingManagementDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(u => u.IsActive).ToListAsync(cancellationToken);
    }
} 