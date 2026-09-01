using System.ComponentModel.DataAnnotations;

namespace Flora.DTOs.Seller
{
    public class CreateSellerDto
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[\u0621-\u063A\u0641-\u064A\u067E\u0686\u0698\u06A9\u06AF\u06CC]+$")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[\u0600-\u06FF]+(?:[ \u200C][\u0600-\u06FF]+)*$")]
        public string LastName { get; set; } = null!;

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
