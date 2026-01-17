using System.ComponentModel.DataAnnotations;
using AtkSystem.Core.Enums;

namespace AtkSystem.Core.Entities;

public class LeaveRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public LeaveType LeaveType { get; set; }

    // Using DateOnly for dates
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public int? ApproverUserId { get; set; }
    public User? ApproverUser { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
