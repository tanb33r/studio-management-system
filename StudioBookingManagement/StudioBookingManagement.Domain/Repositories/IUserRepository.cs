using StudioBookingManagement.Domain.Common;
using StudioBookingManagement.Domain.Entities;
using StudioBookingManagement.Domain.ValueObjects;

namespace StudioBookingManagement.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
} 