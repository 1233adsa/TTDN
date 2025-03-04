using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDN.Models;

namespace TTDN.Controllers
{
    public class BookingController : Controller
    {
        private readonly FootballBookingContext _context;

        public BookingController(FootballBookingContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? dateFilter)
        {
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdClaim.Value);

            try
            {
                var bookings = _context.Bookings
                    .Include(b => b.Field)
                    .Where(b => b.AccountId == accountId);

                if (dateFilter.HasValue)
                {
                    bookings = bookings.Where(b => b.BookingDate.Date == dateFilter.Value.Date);
                }

                var bookingList = bookings.OrderByDescending(b => b.BookingDate).ToList();

                return View(bookingList);
            }
            catch (Exception)
            {
                TempData["Error"] = "Lỗi kết nối";
                return RedirectToAction("Index", "Home");
            }
        }

        // Hủy đặt sân
        public IActionResult Cancel(int bookingId)
        {
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int accountId = int.Parse(accountIdClaim.Value);
            var booking = _context.Bookings
                .Include(b => b.FieldSchedules)
                .FirstOrDefault(b => b.BookingId == bookingId && b.AccountId == accountId);

            if (booking == null)
            {
                TempData["Error"] = "Không tìm thấy đơn đặt sân.";
                return RedirectToAction("Index");
            }

            DateTime now = DateTime.Now.Date;
            DateTime matchDate = booking.BookingDate;

            if (matchDate < now)
            {
                TempData["Error"] = "Không thể hủy sân khi trận đấu đã bắt đầu hoặc kết thúc.";
                return RedirectToAction("Index");
            }

            DateTime cancelDeadline = matchDate.AddDays(-3);
            bool isFullRefund = now < cancelDeadline;

            booking.Status = isFullRefund ? "refunding" : "cancelled";

            var schedules = _context.FieldSchedules
                    .Where(fs => fs.FieldId == booking.FieldId
                              && fs.BookingDate == booking.BookingDate
                              && fs.StartTime == booking.StartTime
                              && fs.EndTime == booking.EndTime)
                    .ToList();

            foreach (var schedule in schedules)
            {
                schedule.Status = "available";
            }

            _context.SaveChanges();

            TempData["Success"] = isFullRefund
                ? "Hủy đặt sân thành công. Bạn sẽ được hoàn lại 100% tiền cọc."
                : "Hủy đặt sân thành công. Bạn sẽ không được hoàn lại tiền cọc.";

            return RedirectToAction("Index");
        }
    }
}
