using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("Shop")]
public partial class Shop
{
    [Key]
    public int Id { get; set; }

    [InverseProperty("Shop")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Shop")]
    public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();

    [InverseProperty("Shop")]
    public virtual ICollection<ShopPhoto> ShopPhotos { get; set; } = new List<ShopPhoto>();
}
