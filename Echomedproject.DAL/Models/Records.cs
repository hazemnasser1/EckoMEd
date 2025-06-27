using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Records
    {
        [Key]
        public int Id { get; set; }
        public string Department { get; set; }
        public string DoctorName { get; set; }

        public DateTime visitDate { get; set; }
        public string HospitalName { get; set; }
        public List<LabTest>? LabTests { get; set; }
        public List<Note>? notes { get; set; }

        public List<Scans>? Scans { get; set; }

        public prescription? prescription { get; set; }

       public Invoice? Invoice { get; set; }
    }
}
