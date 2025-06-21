using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class HospitalSearchViewModel
    {
        [Range(0, double.MaxValue, ErrorMessage = "Distance must be a non-negative number.")]
        public double? Distance { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a non-negative number.")]
        public double? Budget { get; set; }

        [StringLength(100, ErrorMessage = "Department type cannot be longer than 100 characters."), Required]
        public string? Deptype { get; set; }

        [StringLength(100, ErrorMessage = "Insurance name cannot be longer than 100 characters.")]
        public string? Insurance { get; set; }

        [StringLength(100, ErrorMessage = "Hospital name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees.")]
        public double? Lat { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees.")]
        public double? Lang { get; set; }
    }
}
