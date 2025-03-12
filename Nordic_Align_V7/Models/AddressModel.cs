using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Nordic_Align_V7.Models;

public class AddressModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? State { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; } = null!;
}
