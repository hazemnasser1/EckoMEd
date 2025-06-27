using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        public string? type { get; set; }

        public string? Text { get; set; }
        
        public DateTime? dateTime { get; set; }

    }
}
