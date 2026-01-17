using AtkSystem.Core.Entities;
using AtkSystem.Core.Interfaces;
using AtkSystem.Core.Interfaces.Auth;

namespace AtkSystem.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateUserAsync(User user, string password)
    {
        var existing = await _repository.GetByEmployeeIdAsync(user.EmployeeId);
        if (existing != null)
        {
            throw new InvalidOperationException($"社員ID {user.EmployeeId} は既に使用されています。");
        }

        user.PasswordHash = _passwordHasher.HashPassword(password);
        user.IsActive = true;
        user.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user, string? newPassword = null)
    {
        if (!string.IsNullOrEmpty(newPassword))
        {
            user.PasswordHash = _passwordHasher.HashPassword(newPassword);
        }
        
        // Ensure validation logic if needed
        await _repository.UpdateAsync(user);
    }

    public async Task ToggleActiveStatusAsync(int userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null) throw new InvalidOperationException("User not found");

        user.IsActive = !user.IsActive;
        await _repository.UpdateAsync(user);
    }
}
