using System.ComponentModel.DataAnnotations;
using AtkSystem.Core.Enums;

namespace AtkSystem.Core.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public DateOnly WorkDate { get; set; }

    public DateTime? ClockInTime { get; set; }
    public DateTime? ClockOutTime { get; set; }
    public DateTime? BreakStartTime { get; set; } // For tracking current break start

    public int BreakTimeMinutes { get; set; }
    public int TotalWorkMinutes { get; set; }

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Work;

    [MaxLength(200)]
    public string? Remarks { get; set; }
}
