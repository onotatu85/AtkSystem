using AtkSystem.Core.Entities;

namespace AtkSystem.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmployeeIdAsync(string employeeId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user); // For logic delete or hard delete
}
