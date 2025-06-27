using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Echomedproject.PL.ViewModels
{
    public class AddTestViewModel
    {
        [Required(ErrorMessage = "PatientID is required")]

        public string PatientID { get; set; }
        [Required(ErrorMessage = "Test name is required.")]
        [StringLength(100, ErrorMessage = "Test name must be less than 100 characters.")]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Test type is required.")]
        [StringLength(100, ErrorMessage = "Test type must be less than 100 characters.")]
        public string TestType { get; set; }

        [StringLength(500, ErrorMessage = "Note must be less than 500 characters.")]
        public string? Note { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Image file is required.")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
