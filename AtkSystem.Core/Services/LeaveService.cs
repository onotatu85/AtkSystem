using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;
using AtkSystem.Core.Interfaces;

namespace AtkSystem.Core.Services;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _repository;

    public LeaveService(ILeaveRepository repository)
    {
        _repository = repository;
    }

    public async Task<LeaveRequest> ApplyLeaveAsync(int userId, LeaveType type, DateOnly start, DateOnly end, string reason)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date cannot be after end date.");
        }

        var request = new LeaveRequest
        {
            UserId = userId,
            LeaveType = type,
            StartDate = start,
            EndDate = end,
            Reason = reason,
            Status = LeaveStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(request);
        return request;
    }

    public async Task<IEnumerable<LeaveRequest>> GetMyRequestsAsync(int userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
    {
        return await _repository.GetPendingRequestsAsync();
    }

    public async Task ApproveAsync(int requestId, int approverId)
    {
        var request = await _repository.GetByIdAsync(requestId);
        if (request == null) throw new InvalidOperationException("Request not found.");
        
        request.Status = LeaveStatus.Approved;
        request.ApproverUserId = approverId;
        request.ApprovedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(request);
    }

    public async Task RejectAsync(int requestId, int approverId)
    {
        var request = await _repository.GetByIdAsync(requestId);
        if (request == null) throw new InvalidOperationException("Request not found.");

        request.Status = LeaveStatus.Rejected;
        request.ApproverUserId = approverId;
        request.ApprovedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(request);
    }
}
