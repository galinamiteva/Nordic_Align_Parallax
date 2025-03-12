using System.ComponentModel.DataAnnotations.Schema;

namespace Nordic_Align_V7.Models;

public class InvoiceItemModel
{
    public int Id { get; set; }

    public string Item { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public int InvoiceId { get; set; }

    public virtual InvoiceModel Invoice { get; set; } = null!;
}
