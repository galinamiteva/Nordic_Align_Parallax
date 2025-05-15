



using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.Models;

public class OrderModel
{
    public int Id { get; set; }
    [Display(Name = "Namn", ResourceType = typeof(Resource))]
    [MaxLength(50)]
    [Required]
    public string Sender { get; set; } = null!;
    [Display(Name = "Beställningsdatum", ResourceType = typeof(Resource))]

    [Required]
    public DateTime OrderDate { get; set; }
    [Display(Name = "Leveransdatum", ResourceType = typeof(Resource))]

    [Required]
    public DateTime DeliveryDate { get; set; }
    [Display(Name = "Pris", ResourceType = typeof(Resource))]

    [Precision(15, 2)]
    [Required]
    public decimal Price { get; set; }
    public int? CourierId { get; set; }

    [Display(Name = "Mottagare", ResourceType = typeof(Resource))]
    [Required(ErrorMessage = "Du måste välja en mottagare.")]
    public int? RecepientId { get; set; }
    public int? TransportId { get; set; }

    public virtual CourierModel Courier { get; set; } = null!;
    public virtual RecepientModel Recepient { get; set; } = null!;
    public virtual TranportModel Transport { get; set; } = null!;
    public virtual List<WarehouseModel> Warehouses { get; set; } = null!;
}


