using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class PharmacySearchViewModel
    {
        [Range(0, double.MaxValue, ErrorMessage = "Distance must be a non-negative number.")]
        public double? Distance { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees.")]
        public double? Lat { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees.")]
        public double? Lang { get; set; }
    }
}

