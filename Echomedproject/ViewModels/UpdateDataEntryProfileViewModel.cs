using System.ComponentModel.DataAnnotations;

public class UpdateDataEntryProfileViewModel
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    public string City { get; set; }
}
