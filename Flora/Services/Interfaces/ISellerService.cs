using Flora.DTOs.Buyer;
using Flora.DTOs.Seller;

namespace Flora.Services.Interfaces
{
    public interface ISellerService
    {
        //اول چیزی که میده و جایگاه دوم چیزی که میگیره
        Task<CreateSellerResultDto?> CreateAsync(CreateSellerDto dto);
    }
}
