using Nordic_Align_V7.Models;

namespace Nordic_Align_V7.ViewModels;

public class InvoiceViewModel
{
    public string InvoiceNumber { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public string? Comment { get; set; }
    public string? VatNumber { get; set; }


    // Customer Information
    public string CompanyName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? State { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    // Invoice Items
    public List<InvoiceItemModel> Items { get; set; } = new List<InvoiceItemModel>();
}
