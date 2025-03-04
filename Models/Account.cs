using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTDN.Models;

public partial class Account
{
    public int AccountId { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public string Role { get; set; } = "user";

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string phone {  get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
