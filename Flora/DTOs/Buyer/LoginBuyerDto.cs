using System.ComponentModel.DataAnnotations;

namespace Flora.DTOs.Buyer
{
    public class LoginBuyerDto
    {
        [Required]
        [RegularExpression(@"^09[0-9]{9}$")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};:'"",.<>/?\\|`~]+$")]
        public string Password { get; set; } = null!;
    }
}