using System;
using System.Collections.Generic;

namespace Flora.Models;

public partial class Order
{
    public int Id { get; set; }

    public bool ForAnother { get; set; }

    public string? RecipientFirstName { get; set; }

    public string? RecipientLastName { get; set; }

    public string? RecipientPhoneNumber { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public string? SenderName { get; set; }

    public string? SenderText { get; set; }

    public string Province { get; set; } = null!;

    public string City { get; set; } = null!;

    public string ExactAddress { get; set; } = null!;

    public string? PlateNumber { get; set; }

    public string? UnitNumber { get; set; }

    public string? Title { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public string? PostalCode { get; set; }

    public int BuyerId { get; set; }

    public int SendId { get; set; }

    public virtual Buyer Buyer { get; set; } = null!;

    public virtual ICollection<MidorderProduct> MidorderProducts { get; set; } = new List<MidorderProduct>();

    public virtual Send Send { get; set; } = null!;
}
