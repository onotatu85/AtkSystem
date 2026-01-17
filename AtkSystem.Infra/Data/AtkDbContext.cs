using AtkSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtkSystem.Infra.Data;

public class AtkDbContext : DbContext
{
    public AtkDbContext(DbContextOptions<AtkDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>()
            .HasIndex(u => u.EmployeeId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        // AttendanceRecord Configuration
        modelBuilder.Entity<AttendanceRecord>()
            .HasIndex(a => new { a.UserId, a.WorkDate })
            .IsUnique();

        // LeaveRequest Configuration
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(l => l.User)
            .WithMany(u => u.LeaveRequests)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LeaveRequest>()
            .HasOne(l => l.ApproverUser)
            .WithMany()
            .HasForeignKey(l => l.ApproverUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
