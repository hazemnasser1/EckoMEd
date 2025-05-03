using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Echomedproject.DAL.Models
{
    public class Users : IdentityUser
    {
        public string Gender { get; set; }
        public string? FirstName { get; set; }
        public DateTime DateOBirth { get; set; }

        public string? LastName { get; set; }
        public string imagePath { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string country { get; set; }


    }
}
