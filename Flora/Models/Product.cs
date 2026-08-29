using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("Product")]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    public int ShopId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(250)]
    public string Description { get; set; } = null!;

    public int Stock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<MidorderProduct> MidorderProducts { get; set; } = new List<MidorderProduct>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductPhoto> ProductPhotos { get; set; } = new List<ProductPhoto>();

    [ForeignKey("ShopId")]
    [InverseProperty("Products")]
    public virtual Shop Shop { get; set; } = null!;
}
