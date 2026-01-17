using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;
using AtkSystem.Core.Interfaces;
using AtkSystem.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AtkSystem.Infra.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly AtkDbContext _context;

    public LeaveRepository(AtkDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LeaveRequest request)
    {
        _context.LeaveRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(LeaveRequest request)
    {
        _context.LeaveRequests.Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId)
    {
        return await _context.LeaveRequests
            .Include(l => l.ApproverUser)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
    {
        return await _context.LeaveRequests
            .Include(l => l.User)
            .Where(l => l.Status == LeaveStatus.Pending)
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<LeaveRequest?> GetByIdAsync(int id)
    {
        return await _context.LeaveRequests
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}
