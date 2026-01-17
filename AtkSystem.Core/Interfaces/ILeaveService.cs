using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;

namespace AtkSystem.Core.Interfaces;

public interface ILeaveService
{
    Task<LeaveRequest> ApplyLeaveAsync(int userId, LeaveType type, DateOnly start, DateOnly end, string reason);
    Task<IEnumerable<LeaveRequest>> GetMyRequestsAsync(int userId);
    Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync();
    Task ApproveAsync(int requestId, int approverId);
    Task RejectAsync(int requestId, int approverId);
}
