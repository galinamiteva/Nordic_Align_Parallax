using System.ComponentModel.DataAnnotations;

namespace Nordic_Align_V7.ViewModels;

public class ContactViewModel
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }= null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }= null!;

    [Required]
    [Display(Name = "Message")]
    public string Message { get; set; }= null!;

}
