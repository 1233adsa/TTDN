using System;
using System.Collections.Generic;

namespace TTDN.Models;

public partial class Price
{
    public int PriceId { get; set; }

    public int? FieldId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public decimal Price1 { get; set; }

    public virtual Field? Field { get; set; }
}
