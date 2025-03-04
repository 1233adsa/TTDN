using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTDN.Models;

public partial class FieldSchedule
{
    [Key]
    public int ScheduleId { get; set; }

    [ForeignKey("Field")]
    public int? FieldId { get; set; }

    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly StartTime { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly EndTime { get; set; }



    [Required]
    public string Status { get; set; } = null!;

    [ForeignKey("Booking")]
    public int? BookingId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Field? Field { get; set; }
}
