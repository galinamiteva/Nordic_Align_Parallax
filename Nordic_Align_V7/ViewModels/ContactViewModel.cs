using System.ComponentModel.DataAnnotations;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.ViewModels;

public class ContactViewModel
{
    [Required]
    [Display(Name = "Namn", ResourceType = typeof(Resource))]

    public string Name { get; set; }= null!;

    [Required]
    [EmailAddress]
    [Display(Name = "E_Post", ResourceType = typeof(Resource))]

    public string Email { get; set; }= null!;

    [Required]
    [Display(Name = "Meddelande", ResourceType = typeof(Resource))]

    public string Message { get; set; }= null!;

}
