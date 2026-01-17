using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;
using AtkSystem.Core.Interfaces;

namespace AtkSystem.Core.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repository;

    public AttendanceService(IAttendanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<AttendanceRecord> ClockInAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var record = await _repository.GetByUserIdAndDateAsync(userId, today);

        if (record != null)
        {
            throw new InvalidOperationException("Already clocked in today.");
        }

        record = new AttendanceRecord
        {
            UserId = userId,
            WorkDate = today,
            ClockInTime = DateTime.Now,
            Status = AttendanceStatus.Work
        };

        await _repository.AddAsync(record);
        return record;
    }

    public async Task<AttendanceRecord> ClockOutAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var record = await _repository.GetByUserIdAndDateAsync(userId, today);

        if (record == null)
        {
            throw new InvalidOperationException("No attendance record found for today.");
        }

        if (record.ClockOutTime.HasValue)
        {
            throw new InvalidOperationException("Already clocked out.");
        }

        record.ClockOutTime = DateTime.Now;
        record.Status = AttendanceStatus.Work; // Or keep as is? If they clock out, status might be just 'Work' (completed) or stay same.

        // Calculate total work minutes (Simple: Out - In - Break)
        if (record.ClockInTime.HasValue)
        {
            var duration = record.ClockOutTime.Value - record.ClockInTime.Value;
            record.TotalWorkMinutes = (int)duration.TotalMinutes - record.BreakTimeMinutes;
        }

        await _repository.UpdateAsync(record);
        return record;
    }

    public async Task<AttendanceRecord> StartBreakAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var record = await _repository.GetByUserIdAndDateAsync(userId, today);

        if (record == null) throw new InvalidOperationException("Not clocked in.");
        if (record.ClockOutTime.HasValue) throw new InvalidOperationException("Already clocked out.");
        if (record.Status == AttendanceStatus.OnBreak) throw new InvalidOperationException("Already on break.");

        record.Status = AttendanceStatus.OnBreak;
        record.BreakStartTime = DateTime.Now;

        await _repository.UpdateAsync(record);
        return record;
    }

    public async Task<AttendanceRecord> EndBreakAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var record = await _repository.GetByUserIdAndDateAsync(userId, today);

        if (record == null) throw new InvalidOperationException("Not clocked in.");
        if (record.Status != AttendanceStatus.OnBreak) throw new InvalidOperationException("Not on break.");

        var breakEnd = DateTime.Now;
        if (record.BreakStartTime.HasValue)
        {
            var breakDuration = breakEnd - record.BreakStartTime.Value;
            record.BreakTimeMinutes += (int)breakDuration.TotalMinutes;
        }

        record.Status = AttendanceStatus.Work;
        record.BreakStartTime = null;

        await _repository.UpdateAsync(record);
        return record;
    }

    public async Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsAsync(int userId, int year, int month)
    {
        return await _repository.GetMonthlyRecordsAsync(userId, year, month);
    }

    public async Task<IEnumerable<AttendanceRecord>> GetMonthlyRecordsForAllUsersAsync(int year, int month)
    {
        return await _repository.GetMonthlyRecordsForAllUsersAsync(year, month);
    }

    public async Task<AttendanceRecord?> GetTodayRecordAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        return await _repository.GetByUserIdAndDateAsync(userId, today);
    }
}
