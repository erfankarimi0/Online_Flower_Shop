using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class Shop
{
    public int Id { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Seller> Sellers { get; set; } = new List<Seller>();

    public virtual ICollection<ShopPhoto> ShopPhotos { get; set; } = new List<ShopPhoto>();
}
