using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class CapacityViewModel
    {
        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Number of doctors is required.")]
        public int NumOfDoctors { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; }
    }
}
