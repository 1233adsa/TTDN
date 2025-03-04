using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TTDN.Models;

public class BookingCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    //private readonly FootballBookingContext _context;

    public BookingCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<FootballBookingContext>();

                // Lấy danh sách các đơn "pending" đã quá hạn
                var expiredBookings = await _context.Bookings
                    .Where(b => b.Status == "pending" && b.DueTime < DateTime.Now)
                    .ToListAsync();

                if (expiredBookings.Any())
                {
                    foreach (var booking in expiredBookings)
                    {
                        booking.Status = "cancelled";

                        // Cập nhật trạng thái sân trong FieldSchedule về "available"
                        var schedule = await _context.FieldSchedules
                            .Where(fs => fs.FieldId == booking.FieldId && fs.StartTime == booking.StartTime && fs.EndTime == booking.EndTime)
                            .FirstOrDefaultAsync();

                        if (schedule != null)
                        {
                            schedule.Status = "available";
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            // Chạy mỗi 10 phút
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}
