using Flora.DTOs.Buyer;

namespace Flora.Services.Interfaces
{
    public interface IBuyerService
    {
        Task<CreateBuyerResultDto?> CreateAsync(CreateBuyerDto dto);
        Task<LoginBuyerResultDto?> LoginAsync(LoginBuyerDto dto);
    }
}