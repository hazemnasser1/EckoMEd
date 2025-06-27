using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echomedproject.DAL.Models
{
    public class Hospitals
    {
        [Key]
        public int Id { get; set; } // Primary Key, Unique Identifier

        [Required(ErrorMessage = "Hospital name is required")]
        [StringLength(100, ErrorMessage = "Hospital name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot be longer than 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid zip code format")]
        public string ZipCode { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Invalid website URL")]
        public string Website { get; set; }

        // Hospital Facilities & Capacity
        [Range(0, int.MaxValue, ErrorMessage = "Total rooms must be a positive number")]
        public int TotalRooms { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Total beds must be a positive number")]
        public int TotalBeds { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "ICU beds must be a positive number")]
        public int ICUBeds { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Emergency rooms must be a positive number")]
        public int EmergencyRooms { get; set; }

        // Geographical coordinates
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Accepted insurances - simple list of insurance provider names
        public ICollection<Insurance> AcceptedInsurances { get; set; } = new List<Insurance>();

        public ICollection<Departments> Departments { get; set; } = new List<Departments>();

        public int DateEntryId { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        public ICollection<AppUsers> Patients { get; set; } = new List<AppUsers>();
    }
}
