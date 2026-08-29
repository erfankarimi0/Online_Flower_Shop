using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("ShopPhoto")]
public partial class ShopPhoto
{
    [Key]
    public int Id { get; set; }

    public int ShopId { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("ShopPhotos")]
    public virtual Shop Shop { get; set; } = null!;
}
