using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("ProductPhoto")]
public partial class ProductPhoto
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductPhotos")]
    public virtual Product Product { get; set; } = null!;
}
