using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TTDN.Models;

namespace TTDN.Controllers
{
    public class PaymentController : Controller
    {
        private readonly FootballBookingContext _context;

        public PaymentController(FootballBookingContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Field)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var payment = new Payment
            {
                BookingId = booking.BookingId,
                PaymentDate = DateTime.Now,
                Amount = booking.TotalPrice,
                Method = "credit_card", // Giá trị mặc định, có thể đổi sau
                Status = "pending"
            };

            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int bookingId, string method)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Kiểm tra trạng thái thanh toán trước khi tạo thanh toán mới
            if (booking.Status != "pending")
            {
                return BadRequest("The booking has already been processed.");
            }

            var payment = new Payment
            {
                BookingId = bookingId,
                PaymentDate = DateTime.Now,
                Amount = booking.TotalPrice,
                Method = method,
                Status = "completed"
            };

            _context.Payments.Add(payment);
            booking.Status = "confirmed";

            // Cập nhật trạng thái của FieldSchedule thành "booked"
            var fieldSchedule = await _context.FieldSchedules
                .Where(fs => fs.FieldId == booking.FieldId && fs.BookingDate == booking.BookingDate)
                .Where(fs => fs.StartTime == booking.StartTime && fs.EndTime == booking.EndTime)
                .FirstOrDefaultAsync();

            if (fieldSchedule != null)
            {
                fieldSchedule.Status = "booked";
                _context.Entry(fieldSchedule).State = EntityState.Modified; // Đảm bảo trạng thái được thay đổi
            }
            else
            {
                return BadRequest("No matching field schedule found.");
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        public IActionResult Success()
        {
            return View();
        }
    }
}
