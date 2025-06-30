using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Echomedproject.DAL.Models
{
    public class Departments
    {
        [Key]
        public int Id { get; set; } // Primary Key, Unique Identifier

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Department name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        public int NomOfDoctors { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Capacity must be a positive number")]
        public int Capacity { get; set; } // Maximum number of patients the department can handle

        // Foreign key to the Hospital (if departments are tied to a specific hospital)
        public int HospitalID { get; set; }

        public double? budget { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        // Navigation property to the Hospital
        public Hospitals? Hospital { get; set; }
        public int TotalPatients { get; set; }  
    }
}
