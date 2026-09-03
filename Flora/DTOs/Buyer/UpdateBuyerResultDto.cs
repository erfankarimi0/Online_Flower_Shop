using Flora.DTOs.Buyer;
using Flora.Enums;

public class UpdateBuyerResultDto
{
    public UpdateProfileStatus Status { get; set; }

    public GetBuyerDto? Buyer { get; set; }

    public string? Token { get; set; }
}