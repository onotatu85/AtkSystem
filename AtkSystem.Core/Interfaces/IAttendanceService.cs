using AtkSystem.Core.Entities;

namespace AtkSystem.Core.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceRecord> ClockInAsync(int userId);
    Task<AttendanceRecord> ClockOutAsync(int userId);
    Task<AttendanceRecord> StartBreakAsync(int userId);
    Task<AttendanceRecord> EndBreakAsync(int userId);
    Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsAsync(int userId, int year, int month);
    Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsForAllUsersAsync(int year, int month);
    Task<AttendanceRecord?> GetTodayRecordAsync(int userId);
}
