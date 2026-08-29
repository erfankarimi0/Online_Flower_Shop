using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("Send")]
public partial class Send
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdateDate { get; set; }

    [StringLength(50)]
    public string SendStatus { get; set; } = null!;

    [StringLength(50)]
    public string SendMethod { get; set; } = null!;

    public DateOnly Date { get; set; }

    [StringLength(50)]
    public string TimeFrame { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Sendprice { get; set; }

    [InverseProperty("Send")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
