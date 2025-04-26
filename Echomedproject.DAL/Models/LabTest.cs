using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class LabTest
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
