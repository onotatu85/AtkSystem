using System.Security.Claims;
using AtkSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtkSystem.Web.Controllers;

[Authorize]
public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
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
        var todayRecord = await _attendanceService.GetTodayRecordAsync(CurrentUserId);
        var monthlyRecords = await _attendanceService.GetMonthlyRecordsAsync(CurrentUserId, DateTime.Now.Year, DateTime.Now.Month);
        
        ViewBag.TodayRecord = todayRecord;
        ViewBag.MonthlyRecords = monthlyRecords;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ClockIn()
    {
        try
        {
            await _attendanceService.ClockInAsync(CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ClockOut()
    {
        try
        {
            await _attendanceService.ClockOutAsync(CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> StartBreak()
    {
        try
        {
            await _attendanceService.StartBreakAsync(CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> EndBreak()
    {
        try
        {
            await _attendanceService.EndBreakAsync(CurrentUserId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
    [HttpGet]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> ExportCsv()
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        var records = await _attendanceService.GetMonthlyRecordsForAllUsersAsync(year, month);
        
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("社員番号,氏名,日付,出勤,退勤,休憩(分),実働(分),ステータス");

        foreach (var r in records)
        {
            csv.AppendLine($"{r.User?.EmployeeId},{r.User?.FullName},{r.WorkDate:yyyy/MM/dd},{r.ClockInTime:HH:mm},{r.ClockOutTime:HH:mm},{r.BreakTimeMinutes},{r.TotalWorkMinutes},{r.Status}");
        }

        return File(System.Text.Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"attendance_{year}_{month}.csv");
    }
}
