using System.ComponentModel.DataAnnotations;
using Echomedproject.DAL.Models;

namespace Echomedproject.PL.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class RecordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(100, ErrorMessage = "Doctor name cannot exceed 100 characters.")]
        public string doctorName { get; set; }


        [Required(ErrorMessage = "Hospital name is required.")]
        [StringLength(100, ErrorMessage = "Hospital name cannot exceed 100 characters.")]
        public string hospitalName { get; set; }

        [Required(ErrorMessage = "Visit date is required.")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string departmentName { get; set; }

        [StringLength(100, ErrorMessage = "Scan name cannot exceed 100 characters.")]
        public string? scanName { get; set; }
        public string scanDescription { get; set; }

        // Optional: You can add validation on file size/type via a custom attribute
        public IFormFile? ScanImage { get; set; }

        public List<MedicineViewModel>? Medicines { get; set; }
    }

}
