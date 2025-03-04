using System;
using System.Collections.Generic;

namespace TTDN.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? AccountId { get; set; }

    public int? FieldId { get; set; }

    public DateTime BookingDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal DepositAmount { get; set; } // Số tiền đặt cọc

    public DateTime DueTime { get; set; } // Hạn chót thanh toán

    public string Status { get; set; } = null!;

    public virtual Account? Account { get; set; }

    public virtual Field? Field { get; set; }

    public virtual ICollection<FieldSchedule> FieldSchedules { get; set; } = new List<FieldSchedule>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
