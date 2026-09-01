using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class Product
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Stock { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<MidorderProduct> MidorderProducts { get; set; } = new List<MidorderProduct>();

    public virtual ICollection<ProductPhoto> ProductPhotos { get; set; } = new List<ProductPhoto>();

    public virtual Shop Shop { get; set; } = null!;
}
