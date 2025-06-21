using System.ComponentModel.DataAnnotations;

public class NotificationResponseViewModel
{
    [Required(ErrorMessage = "Notification ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Notification ID must be a positive number.")]
    public int notficationId { get; set; }

    [Required(ErrorMessage = "State is required.")]
    public bool State { get; set; }
}
