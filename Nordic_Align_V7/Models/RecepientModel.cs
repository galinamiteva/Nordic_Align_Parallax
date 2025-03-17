using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nordic_Align_V7.Models;

public class RecepientModel
{
    public int Id { get; set; }
    [DisplayName("Namn")]
    [MaxLength(30)]
    [Required]
    public string FullName { get; set; } = null!;


    [DisplayName("Postnummer")]
    [Required]
    public int PostalCode { get; set; }
    [DisplayName("Leverans adress")]
    [MaxLength(50)]
    [Required]
    public string DeliveryAddress { get; set; } = null!;
    [DisplayName("Telefon")]
    [MaxLength(15)]
    [Required]
    public string Phone { get; set; } = null!;
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
