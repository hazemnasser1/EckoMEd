using System;
using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class AddScanViewModel
    {
        [Required(ErrorMessage = "PatientID is required")]

        public string PatientID { get; set; }
        [Required(ErrorMessage = "Scan type is required.")]
        [StringLength(100, ErrorMessage = "Scan type must be less than 100 characters.")]
        public string ScanType { get; set; }

        [Required(ErrorMessage = "Scan part is required.")]
        [StringLength(100, ErrorMessage = "Scan part must be less than 100 characters.")]
        public string ScanPart { get; set; }

        [StringLength(500, ErrorMessage = "Note must be less than 500 characters.")]
        public string? Note { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Image file is required.")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
