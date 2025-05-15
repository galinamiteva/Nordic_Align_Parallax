using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.Models;

public class WarehouseModel
{
    public int Id { get; set; }
    [Display(Name = "Lagernamn", ResourceType = typeof(Resource))]

    [MaxLength(50)]
    [Required]
    public string Name { get; set; } = null!;
    [Display(Name = "Adress", ResourceType = typeof(Resource))]

    [MaxLength(50)]
    [Required]
    public string Address { get; set; } = null!;
    [Display(Name = "Telefon", ResourceType = typeof(Resource))]

    [MaxLength(15)]
    [Required]
    public string Phone { get; set; } = null!;
    [Display(Name = "Kapacitet", ResourceType = typeof(Resource))]

    [Required]
    public int Capacity { get; set; }
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
