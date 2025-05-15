using System.ComponentModel.DataAnnotations;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
    [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "E_Post", ResourceType = typeof(Resource))]

    public string Email { get; set; } = null!;

    [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord", ResourceType = typeof(Resource))]

    public string Password { get; set; } = null!;
}
