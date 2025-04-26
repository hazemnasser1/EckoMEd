using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for Hospital
        public int HospitalId { get; set; }
        public Hospitals Hospital { get; set; }

        // Foreign key for Department
        public int DepartmentId { get; set; }
        public Departments Department { get; set; }
    }
}
