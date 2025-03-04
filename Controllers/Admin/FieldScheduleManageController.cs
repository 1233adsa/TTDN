using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDN.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TTDN.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class FieldScheduleManageController : Controller
    {
        private readonly FootballBookingContext _context;

        public FieldScheduleManageController(FootballBookingContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(DateTime? selectedDate, int? fieldId, string status)
        {
            DateTime date = selectedDate ?? DateTime.Today;
            ViewBag.SelectedDate = date;

            var query = _context.FieldSchedules
                .Include(fs => fs.Field)
                .AsQueryable();

            // Lọc theo ngày đặt sân
            query = query.Where(fs => fs.BookingDate == date);

            // Lọc theo sân (nếu có)
            if (fieldId.HasValue)
            {
                query = query.Where(fs => fs.FieldId == fieldId);
            }

            // Lọc theo trạng thái (nếu có)
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(fs => fs.Status == status);
            }

            var schedules = await query.ToListAsync();
            ViewBag.Fields = _context.Fields.ToList(); // Lấy danh sách sân để hiển thị trong dropdown

            return View(schedules);
        }


        public IActionResult Create()
        {
            ViewBag.Fields = _context.Fields.Where(f => f.Status == "Active").ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FieldSchedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.FieldSchedules.Add(schedule);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Lịch sân đã được thêm thành công.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fields = _context.Fields.Where(f => f.Status == "Active").ToList();
            return View(schedule);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _context.FieldSchedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewBag.Fields = _context.Fields.Where(f => f.Status == "Active").ToList();
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FieldSchedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _context.Update(schedule);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Lịch sân đã được cập nhật.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fields = _context.Fields.Where(f => f.Status == "Active").ToList();
            return View(schedule);
        }

        // Hiển thị trang xác nhận xóa lịch sân
public async Task<IActionResult> Delete(int id)
{
    var schedule = await _context.FieldSchedules
        .Include(fs => fs.Field) // Bao gồm thông tin sân
        .Include(fs => fs.Booking) // Kiểm tra xem có khách đặt hay không
        .FirstOrDefaultAsync(fs => fs.ScheduleId == id);

    if (schedule == null)
    {
        return NotFound();
    }

    return View(schedule); // Hiển thị trang xác nhận xóa
}

// Xử lý xóa sau khi xác nhận
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var schedule = await _context.FieldSchedules
        .Include(fs => fs.Booking)
        .FirstOrDefaultAsync(fs => fs.ScheduleId == id);

    if (schedule == null)
    {
        return NotFound();
    }

    // Kiểm tra nếu lịch sân đã có người đặt thì không thể xóa
    if (schedule.Booking != null)
    {
        TempData["ErrorMessage"] = "Không thể xóa vì đã có người đặt.";
        return RedirectToAction(nameof(Index));
    }

    _context.FieldSchedules.Remove(schedule);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Lịch sân đã được xóa thành công.";
    return RedirectToAction(nameof(Index));
}

    }

}
