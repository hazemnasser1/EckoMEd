using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class PatientHospital
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int HospitalId { get; set; }
        public bool IsAdmitted { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

        public DateTime? LeaveDate { get; set; }
    }
}
