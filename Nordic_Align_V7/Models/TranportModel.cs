using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nordic_Align_V7.Models;

public class TranportModel
{
    public int Id { get; set; }
    [DisplayName("RegNo")]
    [MaxLength(30)]
    [Required]
    public string TransportNumber { get; set; } = null!;
    [DisplayName("Bilmärke")]
    [MaxLength(30)]
    [Required]
    public string Brand { get; set; } = null!;
    [DisplayName("Modell")]
    [MaxLength(30)]
    [Required]
    public string Model { get; set; } = null!;
    [DisplayName("RegDatum")]
    [Required]
    public DateTime RegistrationDate { get; set; }
    [DisplayName("Färg")]
    [MaxLength(30)]
    [Required]
    public string Color { get; set; } = null!;
    [DisplayName("Lastförmåga")]
    [Required]
    public int Capacity { get; set; }
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
