namespace Nordic_Align_V7.Models;

public class InvoiceModel
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Comment { get; set; }
    public string? VatNumber { get; set; }

    public AddressModel CustomerAddress { get; set; } = null!;
    public List<InvoiceItemModel> Items { get; set; } = null!;

}
