using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class DataEntry
    {
        [Key]
        public int Id { get; set; } // Primary Key, Unique Identifier

        [Required(ErrorMessage = "Entry date is required")]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }

        [Required(ErrorMessage = "Total patients is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Total patients must be a positive number")]
        public int TotalPatients { get; set; }

        public int HospitalID { get; set; }

        // Navigation property to Hospital
        public Hospitals Hospital { get; set; }


        // Dictionary to store the number of patients in each department
        public ICollection<Departments> Departments { get; set; } = new List<Departments>();
        
    }
}
