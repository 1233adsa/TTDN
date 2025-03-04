using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TTDN.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<FootballBookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FootballBookingDB")));

// Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Trang đăng nhập nếu chưa đăng nhập
        options.LogoutPath = "/Account/Logout"; // Trang đăng xuất
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang lỗi quyền hạn
    });

builder.Services.AddAuthorization(); // Kích hoạt Authorization
builder.Services.AddHostedService<BookingCleanupService>();

// Controller + View
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // 💡 Kích hoạt xác thực người dùng
app.UseAuthorization();  // 💡 Kích hoạt kiểm tra quyền

// 🔥 Middleware điều hướng Admin khi đăng nhập lại
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        var path = context.Request.Path.Value.ToLower();

        // Nếu là admin mà đang ở Home thì redirect về Dashboard
        if (userRole == "admin" && (path == "/" || path == "/home/index"))
        {
            context.Response.Redirect("/Dashboard/Index");
            return;
        }
    }
    await next();
});


// 🚀 Định tuyến chính
app.UseEndpoints(endpoints =>
{
    // Định tuyến mặc định
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Định tuyến cho Admin
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");
});

app.Run();
