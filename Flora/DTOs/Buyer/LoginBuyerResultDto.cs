using System.ComponentModel.DataAnnotations;

namespace Flora.DTOs.Buyer
{
    public class LoginBuyerResultDto
    {
        public string Token { get; set; } = null!;
    }
}