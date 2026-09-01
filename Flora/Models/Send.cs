using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class Send
{
    public int Id { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public string SendStatus { get; set; } = null!;

    public string SendMethod { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string TimeFrame { get; set; } = null!;

    public decimal Sendprice { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
