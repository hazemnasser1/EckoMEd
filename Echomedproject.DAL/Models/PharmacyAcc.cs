using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class PharmacyAcc
    {
        [Key]
        public int Id { get; set; } // Primary Key, Unique Identifier
        [Required(ErrorMessage = "Entry date is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Entry date is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Entry date is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Entry date is required")]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }

        public Pharmacy? Pharmacy { get; set; }
        public List<Notification>? Notifications { get; set; }

        
    }
}
