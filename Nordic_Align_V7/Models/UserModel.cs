using Microsoft.AspNetCore.Identity;
using Nordic_Align_V7.Resources;
using System.ComponentModel.DataAnnotations;

namespace Nordic_Align_V7.Models;

public class UserModel : IdentityUser
{
    [Required]
    [Display(Name = "Förnamn", ResourceType = typeof(Resource))]

    [ProtectedPersonalData]

    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Efternamn", ResourceType = typeof(Resource))]

    [ProtectedPersonalData]

    public string LastName { get; set; } = null!;
}
