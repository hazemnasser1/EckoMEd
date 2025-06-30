using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class PharmacyResultViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Hospital name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public string pharmacyID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Distance must be a non-negative number.")]
        public double Distance { get; set; }

        public string phonenumber {  get; set; }

    }
}
