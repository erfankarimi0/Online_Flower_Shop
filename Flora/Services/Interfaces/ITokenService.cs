using Flora.Models;

namespace Flora.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Buyer buyer);
    }
}