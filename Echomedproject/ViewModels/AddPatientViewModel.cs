using System;
using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class AddPatientViewModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name must be less than 100 characters.")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Date and time are required.")]
        public DateTime DateTime { get; set; }

         
        [Required(ErrorMessage = "Doctor Name are required.")]
        public string DoctorName { get; set; }

    }
}
