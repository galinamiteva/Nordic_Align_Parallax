using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.Models;

public class RecepientModel
{
    public int Id { get; set; }
    [Display(Name = "Namn", ResourceType = typeof(Resource))]


    [MaxLength(30)]
    [Required]
    public string FullName { get; set; } = null!;

    [Display(Name = "Postnummer", ResourceType = typeof(Resource))]

    [Required]
    public int PostalCode { get; set; }
    [Display(Name = "Leveransadress", ResourceType = typeof(Resource))]

    [MaxLength(50)]
    [Required]
    public string DeliveryAddress { get; set; } = null!;
    [Display(Name = "Telefon", ResourceType = typeof(Resource))]

    [MaxLength(15)]
    [Required]
    public string Phone { get; set; } = null!;
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
