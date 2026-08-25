using Flora.DTOs.Buyer;

namespace Flora.Services.Interfaces
{
    public interface IBuyerService
    {
        Task<bool> CreateAsync(CreateBuyerDto dto);
    }
}


