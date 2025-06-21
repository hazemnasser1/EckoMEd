using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public string PatientName { get; set; }
        public DateTime created { get; set; }
        public string DoctorName { get; set; }
        public List<Charge> Charges { get; set; }
        public double Total { get; set; }

    }
}
