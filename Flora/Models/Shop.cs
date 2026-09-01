using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class Shop
{
    public int Id { get; set; }

    public int SellerId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Seller Seller { get; set; } = null!;

    public virtual ICollection<ShopPhoto> ShopPhotos { get; set; } = new List<ShopPhoto>();
}
