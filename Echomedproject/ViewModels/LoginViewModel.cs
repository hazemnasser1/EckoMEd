using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,20}$",
           ErrorMessage = "Password must include uppercase, lowercase, number, and special character.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(User|DataEntry)$", ErrorMessage = "Not a Valid Role.")]
        public string role { get; set; }
        public bool rememberMe { get; set; }
    }
}
