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

        public DateTime? DateOfbirth { get; set; }

        public string PatientId { get; set; }
        public string? patientName { get; set; }

        public string? Gender { get; set; }

        public int HospitalId { get; set; }
        public string Department { get; set; }
        public string State { get; set; }

        public Records record { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime? LeaveDate { get; set; }
    }
}
