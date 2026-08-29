using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("Seller")]
public partial class Seller
{
    [Key]
    public int Id { get; set; }

    public int ShopId { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("Sellers")]
    public virtual Shop Shop { get; set; } = null!;
}
