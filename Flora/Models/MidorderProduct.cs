using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("MIDOrderProduct")]
public partial class MidorderProduct
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("MidorderProducts")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("MidorderProducts")]
    public virtual Product Product { get; set; } = null!;
}
