using Domain.Entity;

namespace Domain.Interfaces
{
    /// <summary>
    /// Defines repository operations for Role entity.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets all roles asynchronously.
        /// </summary>
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets a role by identifier asynchronously.
        /// </summary>
        Task<Role?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Gets a role by name asynchronously.
        /// </summary>
        Task<Role?> GetByNameAsync(string name, CancellationToken ct = default);

        /// <summary>
        /// Adds a new role asynchronously.
        /// </summary>
        Task AddAsync(Role role, CancellationToken ct = default);

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        void Update(Role role);

        /// <summary>
        /// Removes a role.
        /// </summary>
        void Remove(Role role);

        /// <summary>
        /// Saves changes asynchronously.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        /// <summary>
        /// Checks if a role exists by identifier asynchronously.
        /// </summary>
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Checks if a role name already exists asynchronously.
        /// </summary>
        Task<bool> RoleNameExistsAsync(string name, CancellationToken ct = default);
    }
}
