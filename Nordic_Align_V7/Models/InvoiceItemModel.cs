using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nordic_Align_V7.Models;

public class InvoiceItemModel
{
    public int Id { get; set; }

    public string Item { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Pris måste vara större än 0.")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Antal måste vara minst 1.")]
    public int Quantity { get; set; }

    public int InvoiceId { get; set; }

    public virtual InvoiceModel Invoice { get; set; } = null!;
}
