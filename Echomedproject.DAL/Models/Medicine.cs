using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Medicine
    {
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string frequency { get; set; }
        public string? Timing { get; set; }
        public DateTime? MedDate { get; set; }

        public string Duration { get; set; }
        public string DoctorNotes { get; set; }
    }
}
