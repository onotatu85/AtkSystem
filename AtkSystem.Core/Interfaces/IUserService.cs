using AtkSystem.Core.Entities;

namespace AtkSystem.Core.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task CreateUserAsync(User user, string password);
    Task UpdateUserAsync(User user, string? newPassword = null);
    Task ToggleActiveStatusAsync(int userId);
}
