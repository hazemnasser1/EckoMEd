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
        public string Name { get; set; }
        public LabTest LabTest { get; set; }

        public Scans Scan { get; set; }

        public prescription prescription { get; set; }

       public Invoice Invoice { get; set; }
    }
}
