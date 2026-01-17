using UserEntity = AtkSystem.Core.Entities.User;
using AtkSystem.Core.Interfaces;
using AtkSystem.Infra.Data;
using AtkSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AtkSystem.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly AtkDbContext _context; // Ideally use DepartmentService, but direct context for simple list here

    public UserController(IUserService userService, AtkDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
        return View(new UserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (string.IsNullOrEmpty(model.Password))
        {
            ModelState.AddModelError("Password", "新規作成時はパスワードが必須です");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            return View(model);
        }

        try
        {
            var user = new UserEntity
            {
                EmployeeId = model.EmployeeId,
                FullName = model.FullName,
                Role = model.Role,
                DepartmentId = model.DepartmentId,
                IsActive = model.IsActive // Usually true for Create
            };

            await _userService.CreateUserAsync(user, model.Password!);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        var model = new UserViewModel
        {
            Id = user.Id,
            EmployeeId = user.EmployeeId,
            FullName = user.FullName,
            Role = user.Role,
            DepartmentId = user.DepartmentId,
            IsActive = user.IsActive,
            DepartmentName = user.Department?.Name
        };

        ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", user.DepartmentId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel model)
    {
        if (id != model.Id) return BadRequest();

        // Password is optional during edit
        if (ModelState.Keys.Contains("Password") && string.IsNullOrEmpty(model.Password))
        {
             ModelState.Remove("Password");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", model.DepartmentId);
            return View(model);
        }

        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Role = model.Role;
            user.DepartmentId = model.DepartmentId;
            user.IsActive = model.IsActive;

            // Update user info and optionally password
            await _userService.UpdateUserAsync(user, model.Password);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", model.DepartmentId);
            return View(model);
        }
    }
}
