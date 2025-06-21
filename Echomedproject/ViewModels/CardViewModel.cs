using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Echomedproject.PL.ViewModels
{
    public class CardViewModel
    {
        [Required(ErrorMessage = "Card number is required.")]
        [CreditCard(ErrorMessage = "Invalid card number.")]
        [StringLength(19, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 19 digits.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration date is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Expiration date must be in MM/YY format.")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVC is required.")]
        [Range(100, 9999, ErrorMessage = "CVC must be 3 or 4 digits.")]
        public int CVC { get; set; }
    }
}
