using System.ComponentModel.DataAnnotations;

namespace Echomedproject.PL.ViewModels
{
    public class AddNoteViewModel
    {
        [Required(ErrorMessage = "PatientID is required")]
        public string PatientID { get; set; }
        [Required(ErrorMessage = "Note type is required.")]
        [StringLength(100, ErrorMessage = "Note type must be less than 100 characters.")]
        public string NoteType { get; set; }

        [Required(ErrorMessage = "Note content is required.")]
        [StringLength(1000, ErrorMessage = "Note content must be less than 1000 characters.")]
        public string NoteContent { get; set; }
    }
}
