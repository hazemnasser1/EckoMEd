using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class RequestsdisplayViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required.")]
        [StringLength(100, ErrorMessage = "Medicine name can't be longer than 100 characters.")]
        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int qty { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50)]
        public string state { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(100, ErrorMessage = "User name can't be longer than 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone number can't be longer than 20 digits.")]
        public string phoneNum { get; set; }
    }
}
