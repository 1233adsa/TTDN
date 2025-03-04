using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDN.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TTDN.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class CustomerController : Controller
    {
        private readonly FootballBookingContext _context;

        public CustomerController(FootballBookingContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách khách hàng
        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _context.Accounts
                    .Where(a => a.Role == "user") // Chỉ lấy tài khoản khách hàng
                    .Select(a => new CustomerViewModel
                    {
                        AccountId = a.AccountId,
                        Username = a.Username,
                        Email = a.Email,
                        Phone = a.phone
                    })
                    .ToListAsync();

                return View(customers);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Lỗi kết nối! Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }
        }
        // Hiển thị form thêm khách hàng
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account model)
        {
            try
            {
                // Kiểm tra dữ liệu nhập vào
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                    return View(model);
                }

                // Kiểm tra xem username đã tồn tại chưa
                var existingUser = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == model.Username);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Tên tài khoản đã tồn tại!";
                    return View(model);
                }

                // Mã hóa mật khẩu (nếu có hệ thống mã hóa)
                model.Role = "user";
                model.CreatedAt = DateTime.Now;

                _context.Accounts.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Lỗi kết nối! Vui lòng thử lại.";
                return View(model);
            }
        }
    }
}