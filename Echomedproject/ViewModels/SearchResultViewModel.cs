using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class SearchResultViewModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "Hospital name cannot be longer than 100 characters.")]
        public string HospitalName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Distance must be a non-negative number.")]
        public double Distance { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a non-negative number.")]
        public double Budget { get; set; }

        [StringLength(100, ErrorMessage = "Department cannot be longer than 100 characters.")]
        public string Department { get; set; }

        public bool Insurance { get; set; }
    }
}
