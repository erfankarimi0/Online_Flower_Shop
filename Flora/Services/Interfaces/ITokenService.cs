using Flora.Models;

public interface ITokenService
{
    string CreateToken(Buyer buyer);//buyer
    string CreateSellerToken(Seller seller);
}