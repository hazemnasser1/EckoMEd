using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class AppUsers
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNum { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters")]
        public string? Gender { get; set; }

        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
        public int Age { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(100, ErrorMessage = "street cannot be longer than 100 characters")]
        public string? street { get; set; }

        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters")]
        public string? Country { get; set; }
        public string CardNumber { get; set; }

        [ValidInsurance]
        public string? Insurance { get; set; }
        public string ExpirationDate { get; set; }

        public int CVC { get; set; }

        public ICollection<Records> Records { get; set; } = new List<Records>();

        public List<Notification>? notifications { get; set; }




    }


    public class ValidInsuranceAttribute : ValidationAttribute
    {
        private static readonly HashSet<string> _validInsurances = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "CairoCare", "MisrHealth", "NileSecure", "Takaful Egypt", "Al Hayah", "GlobalMed Cairo",
        "HealthNet Egypt", "LifeSecure", "Delta Insurance", "SahhaCare", "SmartHealth", "ShifaPlan"
    };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var insurance = value as string;

            if (string.IsNullOrWhiteSpace(insurance))
                return ValidationResult.Success; // Optional field — skip validation if empty

            if (_validInsurances.Contains(insurance.Trim()))
                return ValidationResult.Success;

            return new ValidationResult("Invalid insurance value. Please select a supported insurance provider.");
        }
    }
}
