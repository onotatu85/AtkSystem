using System.Security.Claims;
using AtkSystem.Core.Enums;
using AtkSystem.Core.Interfaces;
using AtkSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtkSystem.Web.Controllers;

[Authorize]
public class LeaveController : Controller
{
    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    private int CurrentUserId
    {
        get
        {
            var claim = User.FindFirst("UserId");
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
        }
    }

    public async Task<IActionResult> Index()
    {
        var requests = await _leaveService.GetMyRequestsAsync(CurrentUserId);
        return View(requests);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new LeaveRequestViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _leaveService.ApplyLeaveAsync(CurrentUserId, model.LeaveType, model.StartDate, model.EndDate, model.Reason);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Manage()
    {
        var requests = await _leaveService.GetPendingRequestsAsync();
        return View(requests);
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        await _leaveService.ApproveAsync(id, CurrentUserId);
        return RedirectToAction(nameof(Manage));
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        await _leaveService.RejectAsync(id, CurrentUserId);
        return RedirectToAction(nameof(Manage));
    }
}
