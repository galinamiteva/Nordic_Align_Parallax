using System.ComponentModel.DataAnnotations;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Förnamn", ResourceType = typeof(Resource))]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "Efternamn", ResourceType = typeof(Resource))]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
    [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
    [Display(Name = "E_Post", ResourceType = typeof(Resource))]
    public string Email { get; set; } = null!;

    [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
    [DataType(DataType.Password)]
    [Display(Name = "Skapalösenord", ResourceType = typeof(Resource))]
    public string Password { get; set; } = null!;

    [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
    [Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
    [DataType(DataType.Password)]
    [Display(Name = "Bekräftalösenord", ResourceType = typeof(Resource))]
    public string ConfirmPassword { get; set; } = null!;
}
