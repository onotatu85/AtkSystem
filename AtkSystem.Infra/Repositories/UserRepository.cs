using AtkSystem.Core.Entities;
using AtkSystem.Core.Interfaces;
using AtkSystem.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AtkSystem.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AtkDbContext _context;

    public UserRepository(AtkDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Department)
            .OrderBy(u => u.EmployeeId)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmployeeIdAsync(string employeeId)
    {
        return await _context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
