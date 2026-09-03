using Flora.Enums;
using System.ComponentModel.DataAnnotations;

namespace Flora.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        [RegularExpression(@"^09[0-9]{9}$")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};:'"",.<>/?\\|`~]+$")]
        public string Password { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }
    }
}