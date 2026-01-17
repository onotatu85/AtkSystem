using AtkSystem.Core.Entities;
using AtkSystem.Core.Interfaces;
using AtkSystem.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AtkSystem.Infra.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AtkDbContext _context;

    public AttendanceRepository(AtkDbContext context)
    {
        _context = context;
    }

    public async Task<AttendanceRecord?> GetByUserIdAndDateAsync(int userId, DateOnly date)
    {
        return await _context.AttendanceRecords
            .FirstOrDefaultAsync(r => r.UserId == userId && r.WorkDate == date);
    }

    public async Task AddAsync(AttendanceRecord record)
    {
        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AttendanceRecord record)
    {
        _context.AttendanceRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsAsync(int userId, int year, int month)
    {
        return await _context.AttendanceRecords
            .Where(r => r.UserId == userId && r.WorkDate.Year == year && r.WorkDate.Month == month)
            .OrderBy(r => r.WorkDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsForAllUsersAsync(int year, int month)
    {
        return await _context.AttendanceRecords
            .Include(r => r.User)
            .ThenInclude(u => u.Department)
            .Where(r => r.WorkDate.Year == year && r.WorkDate.Month == month)
            .OrderBy(r => r.WorkDate)
            .ThenBy(r => r.User.EmployeeId)
            .ToListAsync();
    }
}

