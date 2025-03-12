using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Nordic_Align_V7.Models;

public class CourierModel
{
    public int Id { get; set; }


    [DisplayName("Förnamn och efternamn")]
    [MaxLength(30)]
    [Required]
    public string FullName { get; set; } = null!;

    [DisplayName("Personnummer")]
    [Required]
    public long Personnummer { get; set; }




    [DisplayName("Anställningsdatum")]
    [Required]

    public DateTime EmploymentDate { get; set; }

    [DisplayName("Start av arbetsdagen")]
    [Required]
    public TimeSpan StartWorkTime { get; set; }
    [DisplayName("Slut på arbetsdagen")]
    [Required]
    public TimeSpan EndWorkTime { get; set; }
    [DisplayName("Ort")]
    [MaxLength(30)]
    [Required]
    public string LivingPlace { get; set; } = null!;
    [DisplayName("Telefon")]
    [MaxLength(15)]
    [Required]
    public string Phone { get; set; } = null!;
    public virtual List<OrderModel> Orders { get; set; } = null!;
}
