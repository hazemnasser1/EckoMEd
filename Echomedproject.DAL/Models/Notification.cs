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

        public string? MedicineName { get; set; }

        public bool? IsExist { get; set; }

        public DateTime DateTime { get; set; }
       
        public string UserName { get; set; }
        public string? PharmacyID { get; set; }


    }
}
