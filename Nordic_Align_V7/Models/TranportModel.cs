using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.Models;

public class TranportModel
{
    public int Id { get; set; }
    [Display(Name = "RegNo", ResourceType = typeof(Resource))]

    [MaxLength(30)]
    [Required]
    public string TransportNumber { get; set; } = null!;
    [Display(Name = "Bilmärke", ResourceType = typeof(Resource))]

    [MaxLength(30)]
    [Required]
    public string Brand { get; set; } = null!;
    [Display(Name = "Modell", ResourceType = typeof(Resource))]

    [MaxLength(30)]
    [Required]
    public string Model { get; set; } = null!;
    
    [Display(Name = "RegDatum", ResourceType = typeof(Resource))]

    [Required]
    public DateTime RegistrationDate { get; set; }
    [Display(Name = "Färg", ResourceType = typeof(Resource))]
    [MaxLength(30)]
    [Required]
    public string Color { get; set; } = null!;
    [Display(Name = "Lastförmåga", ResourceType = typeof(Resource))]
    [Required]
    public int Capacity { get; set; }
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
