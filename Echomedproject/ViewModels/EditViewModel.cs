using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Echomedproject.PL.ViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        public string Lastname { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
