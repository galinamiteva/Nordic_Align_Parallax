using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nordic_Align_V7.Models;

public class WarehouseModel
{
    public int Id { get; set; }
    [DisplayName("Lagernamn")]
    [MaxLength(50)]
    [Required]
    public string Name { get; set; } = null!;
    [DisplayName("Adress")]
    [MaxLength(50)]
    [Required]
    public string Address { get; set; } = null!;
    [DisplayName("Telefon")]
    [MaxLength(15)]
    [Required]
    public string Phone { get; set; } = null!;
    [DisplayName("Kapacitet")]
    [Required]
    public int Capacity { get; set; }
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
