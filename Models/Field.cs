using System;
using System.Collections.Generic;

namespace TTDN.Models;

public partial class Field
{
    public int FieldId { get; set; }

    public string FieldName { get; set; } = null!;

    public string Status {  get; set; } = "available";

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<FieldSchedule> FieldSchedules { get; set; } = new List<FieldSchedule>();

    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();
}
