using AtkSystem.Core.Entities;
using AtkSystem.Core.Enums;
using AtkSystem.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AtkSystem.Infra.Data;

public static class DbInitializer
{
    public static void Initialize(AtkDbContext context)
    {
        context.Database.Migrate();

        if (context.Users.Any())
        {
            return;   // DB has been seeded
        }

        var dept = new Department { Name = "System Development" };
        context.Departments.Add(dept);
        context.SaveChanges();

        var hasher = new AtkSystem.Infra.Services.Auth.BcryptPasswordHasher();

        var users = new User[]
        {
            new User
            {
                EmployeeId = "EMP001",
                FullName = "Admin User",
                PasswordHash = hasher.HashPassword("admin123"),
                Role = Role.Admin,
                DepartmentId = dept.Id
            },
            new User
            {
                EmployeeId = "EMP002",
                FullName = "General User",
                PasswordHash = hasher.HashPassword("user123"),
                Role = Role.General,
                DepartmentId = dept.Id
            }
        };

        foreach (var u in users)
        {
            context.Users.Add(u);
        }
        context.SaveChanges();
    }
}
