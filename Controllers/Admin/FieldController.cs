using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDN.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TTDN.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class FieldController : Controller
    {
        private readonly FootballBookingContext _context;

        public FieldController(FootballBookingContext context)
        {
            _context = context;
        }

        // Danh sách sân
        public async Task<IActionResult> Index()
        {
            var fields = await _context.Fields.ToListAsync();
            return View(fields);
        }

        // Hiển thị form thêm sân
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm sân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Field field)
        {
            if (!ModelState.IsValid)
            {
                return View(field);
            }

            _context.Add(field);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm sân thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form chỉnh sửa
        public async Task<IActionResult> Edit(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        // Xử lý chỉnh sửa sân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Field field)
        {
            if (id != field.FieldId || !ModelState.IsValid)
            {
                return View(field);
            }

            _context.Update(field);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật sân thành công!";
            return RedirectToAction(nameof(Index));
        }
        // Xử lý xóa sân (Gọi bằng AJAX)
        [HttpPost]
        [Route("Admin/Field/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sân!" });
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa sân thành công!" });
        }
    }
}
