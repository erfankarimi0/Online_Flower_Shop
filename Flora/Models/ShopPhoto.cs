using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class ShopPhoto
{
    public int Id { get; set; }

    public int ShopId { get; set; }

    public virtual Shop Shop { get; set; } = null!;
}
