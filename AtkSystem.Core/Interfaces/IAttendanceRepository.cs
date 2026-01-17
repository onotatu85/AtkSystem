using AtkSystem.Core.Entities;

namespace AtkSystem.Core.Interfaces;

public interface IAttendanceRepository
{
    Task<AttendanceRecord?> GetByUserIdAndDateAsync(int userId, DateOnly date);
    Task AddAsync(AttendanceRecord record);
    Task UpdateAsync(AttendanceRecord record);
    Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsAsync(int userId, int year, int month);
    Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsForAllUsersAsync(int year, int month);
}
