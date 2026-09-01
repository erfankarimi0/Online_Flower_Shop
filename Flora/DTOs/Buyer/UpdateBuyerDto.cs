using System.ComponentModel.DataAnnotations;

namespace Flora.DTOs.Buyer
{
    public class UpdateBuyerDto
    {
        [StringLength(50)]
        [RegularExpression(@"^[\u0621-\u063A\u0641-\u064A\u067E\u0686\u0698\u06A9\u06AF\u06CC]+$")] public string? FirstName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[\u0600-\u06FF]+(?:[ \u200C][\u0600-\u06FF]+)*$")]
        public string? LastName { get; set; }

        [RegularExpression(@"^09[0-9]{9}$")]
        public string? PhoneNumber { get; set; }
    }
}
