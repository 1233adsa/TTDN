using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDN.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class FieldScheduleController : Controller
{
    private readonly FootballBookingContext _context;

    public FieldScheduleController(FootballBookingContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(DateTime? selectedDate, int? fieldId, decimal? maxPrice, TimeOnly? startTime, TimeOnly? endTime, string status)
    {
        DateTime date = selectedDate ?? DateTime.Today;
        ViewBag.SelectedDate = date;

        var query = _context.FieldSchedules
            .Where(fs => fs.BookingDate == date)
            .Join(
                _context.Fields.Where(f => f.Status == "Active"),
                fs => fs.FieldId,
                f => f.FieldId,
                (fs, f) => new FieldScheduleViewModel
                {
                    ScheduleId = fs.ScheduleId,
                    FieldId = fs.FieldId ?? 0,
                    FieldName = f.FieldName,
                    BookingDate = fs.BookingDate,
                    StartTime = fs.StartTime,
                    EndTime = fs.EndTime,
                    Status = fs.Status,
                    Price1 = _context.Prices
                        .Where(p => p.FieldId == fs.FieldId && p.StartTime == fs.StartTime && p.EndTime == fs.EndTime)
                        .Select(p => p.Price1)
                        .FirstOrDefault()
                })
            .AsQueryable()
            .OrderBy(fs => fs.FieldId)
            .ThenBy(fs => fs.StartTime);

        if (fieldId.HasValue)
        {
            query = (IOrderedQueryable<FieldScheduleViewModel>)query.Where(fs => fs.FieldId == fieldId.Value);
        }
        if (maxPrice.HasValue)
        {
            query = (IOrderedQueryable<FieldScheduleViewModel>)query.Where(fs => fs.Price1 <= maxPrice.Value);
        }
        if (startTime.HasValue)
        {
            query = (IOrderedQueryable<FieldScheduleViewModel>)query.Where(fs => fs.StartTime >= startTime.Value && fs.EndTime >= startTime.Value);
        }
        if (endTime.HasValue)
        {
            query = (IOrderedQueryable<FieldScheduleViewModel>)query.Where(fs => fs.StartTime <= endTime.Value && fs.EndTime <= endTime.Value);
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = (IOrderedQueryable<FieldScheduleViewModel>)query.Where(fs => fs.Status == status);
        }

        var schedules = await query.ToListAsync();
        return View(schedules);
    }

    private int? GetCurrentAccountId()
    {
        if (User.Identity.IsAuthenticated)
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim != null && int.TryParse(accountIdClaim.Value, out int accountId))
            {
                return accountId;
            }
        }
        return null;
    }

    public async Task<IActionResult> BookFieldConfirmation(int FieldId, DateTime BookingDate, TimeOnly StartTime, TimeOnly EndTime, decimal Price)
    {
        var accountId = GetCurrentAccountId();
        if (accountId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var field = await _context.Fields.FirstOrDefaultAsync(f => f.FieldId == FieldId);
        if (field == null || field.Status != "Active")
        {
            return BadRequest("Sân không khả dụng.");
        }

        var booking = new Booking
        {
            AccountId = accountId.Value,
            FieldId = FieldId,
            BookingDate = BookingDate,
            StartTime = StartTime,
            EndTime = EndTime,
            TotalPrice = Price,
            DepositAmount = Price * 0.3m,
            Status = "pending",
            Field = field
        };

        // Trả về đúng View "BookFieldConfirmation"
        return View("BookFieldConfirmation", booking);
    }


    [HttpPost]
    public async Task<IActionResult> BookField(Booking model)
    {
        var accountId = GetCurrentAccountId();
        if (accountId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        model.AccountId = accountId.Value;
        model.Status = "pending";

        // Cập nhật DueTime = Thời gian hiện tại + 30 phút
        model.DueTime = DateTime.Now.AddMinutes(30);

        _context.Bookings.Add(model);
        await _context.SaveChangesAsync();

        return View(model);
    }

}
