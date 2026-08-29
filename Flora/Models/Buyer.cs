using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("Buyer")]
public partial class Buyer
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(11)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(24)]
    [Unicode(false)]
    public string PasswordSalt { get; set; } = null!;

    [InverseProperty("Buyer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
