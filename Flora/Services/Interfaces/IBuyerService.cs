using Flora.DTOs.Buyer;

namespace Flora.Services.Interfaces
{
    public interface IBuyerService
    {
        //اول چیزی که میده و جایگاه دوم چیزی که میگیره
        Task<CreateBuyerResultDto?> CreateAsync(CreateBuyerDto dto);
        Task<LoginBuyerResultDto?> LoginAsync(LoginBuyerDto dto);
        Task<GetBuyerDto?> GetMeAsync(int buyerId);
        Task<UpdateBuyerResultDto> UpdateMeAsync(int buyerId, UpdateBuyerDto dto);
    }
}