using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDN.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TTDN.Controllers
{
    public class AccountController : Controller
    {
        private readonly FootballBookingContext _context;

        public AccountController(FootballBookingContext context)
        {
            _context = context;
        }

        // Kiểm tra và điều hướng nếu đã đăng nhập
        private IActionResult AutoRedirect()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == "admin")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                return RedirectToAction("Index", "Home");
            }
            return null;
        }

        // Hiển thị trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            var redirect = AutoRedirect();
            if (redirect != null) return redirect;
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(Account account)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Username == account.Username);
                if (existingAccount != null)
                {
                    TempData["ErrorMessage"] = "Tài khoản đã tồn tại.";
                    return View(account);
                }

                account.Role = "user";
                account.CreatedAt = DateTime.Now;

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(account);
        }

        // Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            var redirect = AutoRedirect();
            if (redirect != null) return redirect;
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Username == username && a.Password == password);

            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim("AccountId", account.AccountId.ToString()),
                    new Claim(ClaimTypes.Role, account.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                TempData["SuccessMessage"] = "Đăng nhập thành công!";

                return account.Role == "admin"
                    ? RedirectToAction("Index", "Dashboard")
                    : RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Sai tên đăng nhập hoặc mật khẩu.";
            return View();
        }

        // Hiển thị trang chỉnh sửa tài khoản
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var accountId = User.FindFirst("AccountId")?.Value;
            if (accountId == null)
            {
                return RedirectToAction("Login");
            }

            var account = await _context.Accounts.FindAsync(int.Parse(accountId));
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // Xử lý chỉnh sửa tài khoản
        [HttpPost]
        public async Task<IActionResult> Edit(int accountId, string email, string phone, string oldPassword, string newPassword, string confirmPassword)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            account.Email = email;
            account.phone = phone;

            if (!string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword))
            {
                if (account.Password != oldPassword)
                {
                    TempData["ErrorMessage"] = "Mật khẩu cũ không đúng.";
                    return View(account);
                }
                if (newPassword != confirmPassword)
                {
                    TempData["ErrorMessage"] = "Mật khẩu mới không trùng khớp.";
                    return View(account);
                }
                account.Password = newPassword;
            }

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
            return RedirectToAction("Edit");
        }

        // Xử lý đăng xuất
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
