using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class AddMedicineViewModel
    {
        [Required(ErrorMessage = "PatientID is required")]

        public string PatientID { get; set; }

        [Required(ErrorMessage = "Medicine name is required.")]
        [StringLength(100, ErrorMessage = "Medicine name must be less than 100 characters.")]
        public string MedicineName { get; set; }

        public string? MedicineNotes { get; set; }

        [Required(ErrorMessage = "Dosage is required.")]
        [StringLength(50, ErrorMessage = "Dosage must be less than 50 characters.")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Frequency is required.")]
        [Range(1, 24, ErrorMessage = "Frequency must be between 1 and 24.")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Timing is required.")]
        [StringLength(100, ErrorMessage = "Timing must be less than 100 characters.")]
        public string Timing { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [StringLength(50, ErrorMessage = "Duration must be less than 50 characters.")]
        public string Duration { get; set; }
    }
}
