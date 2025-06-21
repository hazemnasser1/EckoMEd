using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Scans
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        public string? Description { get; set; }
        public DateTime? Date { get; set; }

        public string ImagePath {  get; set; }
    }
}
