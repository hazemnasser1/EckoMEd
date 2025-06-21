using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Pharmacy
    {

        [Key]
        public int Id { get; set; } // Primary Key, Unique Identifier

        [Required(ErrorMessage = "Hospital name is required")]
        [StringLength(100, ErrorMessage = "Hospital name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Identifier is required")]
        [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters")]
        public string Identifier { get; set; }

        // Geographical coordinates
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
