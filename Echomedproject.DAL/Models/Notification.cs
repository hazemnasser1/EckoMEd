using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public string PharmacyName { get; set; }

        public string? Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public string? Type { get; set; } // optional: e.g., "RequestStatus", "SystemAlert"
    }

}
