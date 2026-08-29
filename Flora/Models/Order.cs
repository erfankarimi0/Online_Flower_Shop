using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Flora.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    public int Id { get; set; }

    public bool ForAnother { get; set; }

    [StringLength(50)]
    public string? RecipientFirstName { get; set; }

    [StringLength(50)]
    public string? RecipientLastName { get; set; }

    [StringLength(11)]
    public string? RecipientPhoneNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdateDate { get; set; }

    [StringLength(50)]
    public string OrderStatus { get; set; } = null!;

    [StringLength(50)]
    public string? SenderName { get; set; }

    [StringLength(250)]
    public string? SenderText { get; set; }

    [StringLength(50)]
    public string Province { get; set; } = null!;

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(100)]
    public string ExactAddress { get; set; } = null!;

    [StringLength(50)]
    public string? PlateNumber { get; set; }

    [StringLength(50)]
    public string? UnitNumber { get; set; }

    [StringLength(50)]
    public string? Title { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    public int BuyerId { get; set; }

    public int SendId { get; set; }

    [ForeignKey("BuyerId")]
    [InverseProperty("Orders")]
    public virtual Buyer Buyer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<MidorderProduct> MidorderProducts { get; set; } = new List<MidorderProduct>();

    [ForeignKey("SendId")]
    [InverseProperty("Orders")]
    public virtual Send Send { get; set; } = null!;
}
