using Flora.DTOs.Buyer;
using Flora.Enums;

namespace Flora.DTOs.Seller
{
    public class UpdateSellerResultDto
    {
        public UpdateProfileStatus Status { get; set; }

        public GetBuyerDto? Buyer { get; set; }

        public string? Token { get; set; }
        public GetSellerDto Seller { get; internal set; }
    }
}
