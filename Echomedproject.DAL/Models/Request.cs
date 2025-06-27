using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Request
    {

        [Key]
        public int Id { get; set; }

        public PharmacyAcc pharmacyAcc { get; set; }

        public string? state { get; set; }

        public AppUsers AppUser { get; set; }
        public DateTime? SentAt { get; set; }

        public string Response {  get; set; }

        public string MedicineName { get; set; }

        public int qty { get; set; }

        public DateTime? ClosedAt { get; set; }


    }
}
