using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class Seller
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
