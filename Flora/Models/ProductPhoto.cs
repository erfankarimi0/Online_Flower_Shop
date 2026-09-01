using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class ProductPhoto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
