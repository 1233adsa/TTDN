// PriceController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TTDN.Models;

namespace TTDN.Controllers.Admin
{
    [Authorize(Roles = "admin")] // Chỉ admin mới có quyền truy cập
    public class PriceController : Controller
    {
        private readonly FootballBookingContext _context;

        public PriceController(FootballBookingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prices = await _context.Prices.Include(p => p.Field).ToListAsync();
            return View(prices);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var price = await _context.Prices.Include(p => p.Field).FirstOrDefaultAsync(p => p.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }
            return View(price);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriceId,FieldId,StartTime,EndTime,Price1")] Price price)
        {
            if (id != price.PriceId)
            {
                return NotFound();
            }

            if (price.Price1 <= 0)
            {
                ModelState.AddModelError("Price1", "Giá phải lớn hơn 0.");
                return View(price);
            }

            try
            {
                _context.Update(price);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật giá thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Lỗi kết nối. Vui lòng thử lại sau.");
                return View(price);
            }
        }
    }
}
