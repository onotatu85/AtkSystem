using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<AtkSystem.Infra.Data.AtkDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Application Services
builder.Services.AddScoped<AtkSystem.Core.Interfaces.IAttendanceRepository, AtkSystem.Infra.Repositories.AttendanceRepository>();
builder.Services.AddScoped<AtkSystem.Core.Interfaces.IAttendanceService, AtkSystem.Core.Services.AttendanceService>();
builder.Services.AddScoped<AtkSystem.Core.Interfaces.ILeaveRepository, AtkSystem.Infra.Repositories.LeaveRepository>();
builder.Services.AddScoped<AtkSystem.Core.Interfaces.ILeaveService, AtkSystem.Core.Services.LeaveService>();
builder.Services.AddScoped<AtkSystem.Core.Interfaces.IUserRepository, AtkSystem.Infra.Repositories.UserRepository>();
builder.Services.AddScoped<AtkSystem.Core.Interfaces.IUserService, AtkSystem.Core.Services.UserService>();
builder.Services.AddScoped<AtkSystem.Core.Interfaces.Auth.IPasswordHasher, AtkSystem.Infra.Services.Auth.BcryptPasswordHasher>();

// Add Authentication
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8); 
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AtkSystem.Infra.Data.AtkDbContext>();
        AtkSystem.Infra.Data.DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
