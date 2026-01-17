using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;

namespace AtkSystem.Core.Interfaces;

public interface ILeaveRepository
{
    Task AddAsync(LeaveRequest request);
    Task UpdateAsync(LeaveRequest request);
    Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId);
    Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync();
    Task<LeaveRequest?> GetByIdAsync(int id);
}
