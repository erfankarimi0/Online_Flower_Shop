using Flora.DTOs.Buyer;
using Flora.Enums;

public class UpdateBuyerResultDto
{
    public UpdateBuyerStatus Status { get; set; }

    public GetBuyerDto? Buyer { get; set; }

    public string? Token { get; set; }
}