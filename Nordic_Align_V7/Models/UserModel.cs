using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Nordic_Align_V7.Models;

public class UserModel : IdentityUser
{
    [Required]
    [Display(Name = "First Name")]
    [ProtectedPersonalData]

    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name")]
    [ProtectedPersonalData]

    public string LastName { get; set; } = null!;
}
