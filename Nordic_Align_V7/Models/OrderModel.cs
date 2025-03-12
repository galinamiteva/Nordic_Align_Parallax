using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nordic_Align_V7.Models;

public class OrderModel
{
    public int Id { get; set; }
    [DisplayName("Avsändare Fullständigt Namn")]
    [MaxLength(50)]
    [Required]
    public string Sender { get; set; } = null!;
    [DisplayName("Beställningsdatum")]
    [Required]
    public DateTime OrderDate { get; set; }
    [DisplayName("Leveransdatum")]
    [Required]
    public DateTime DeliveryDate { get; set; }
    [DisplayName("Pris")]
    [Precision(15, 2)]
    [Required]
    public decimal Price { get; set; }
    public int? CourierId { get; set; }
    public int? RecepientId { get; set; }
    public int? TransportId { get; set; }

    public virtual CourierModel Courier { get; set; } = null!;
    public virtual RecepientModel Recepient { get; set; } = null!;
    public virtual TranportModel Transport { get; set; } = null!;
    public virtual List<WarehouseModel> Warehouses { get; set; } = null!;
}
