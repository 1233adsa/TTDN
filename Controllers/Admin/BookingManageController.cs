using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TTDN.Models;

namespace TTDN.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class BookingManageController : Controller
    {
        private readonly FootballBookingContext _context;

        public BookingManageController(FootballBookingContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách đơn đặt sân (mặc định là hôm nay)
        public async Task<IActionResult> Index(DateTime? selectedDate)
        {
            try
            {
                DateTime dateToView = selectedDate ?? DateTime.Today;
                var bookings = await _context.Bookings
                    .Include(b => b.Account)
                    .Include(b => b.Field)
                    .Where(b => b.BookingDate.Date == dateToView.Date)
                    .ToListAsync();

                if (!bookings.Any())
                {
                    ViewBag.Message = "Không có đơn đặt sân nào trong ngày này.";
                }

                ViewBag.SelectedDate = dateToView;
                return View(bookings);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Lỗi kết nối. Vui lòng thử lại sau.";
                return View();
            }
        }

        // Xác nhận thanh toán
        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.FieldSchedules)
                    .SingleOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                {
                    return NotFound("Không tìm thấy đơn đặt sân.");
                }

                // Cập nhật trạng thái đặt sân thành 'confirmed'
                booking.Status = "confirmed";

                var schedules = await _context.FieldSchedules
                    .Where(fs => fs.FieldId == booking.FieldId
                              && fs.BookingDate == booking.BookingDate
                              && fs.StartTime == booking.StartTime
                              && fs.EndTime == booking.EndTime)
                    .ToListAsync();

                // Cập nhật tất cả lịch sân liên quan thành 'booked'
                foreach (var schedule in schedules)
                {
                    schedule.Status = "booked";
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Lỗi khi xác nhận thanh toán. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }

        // Xác nhận hoàn tiền đặt cọc
        [HttpPost]
        public IActionResult ConfirmRefund(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null || booking.Status != "refunding")
            {
                TempData["Error"] = "Không tìm thấy đơn hủy hợp lệ.";
                return RedirectToAction("Index");
            }

            // Xác nhận hoàn tiền
            booking.Status = "refunded";
            _context.SaveChanges();

            TempData["Success"] = "Đã xác nhận hoàn tiền đặt cọc cho khách.";
            return RedirectToAction("Index");
        }
    }
}
