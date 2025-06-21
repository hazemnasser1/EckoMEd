using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class prescription
    {
        public int Id { get; set; }
        public List<Medicine> medicines { get; set; }
       
    }
}
