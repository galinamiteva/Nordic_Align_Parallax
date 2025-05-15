using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Nordic_Align_V7.Resources;

namespace Nordic_Align_V7.Models;

public class CourierModel
{

    public int Id { get; set; }

    [Display(Name = "Namn", ResourceType = typeof(Resource))]
    [MaxLength(30)]
    [Required]
    public string FullName { get; set; } = null!;

    [Display(Name = "Personnummer", ResourceType = typeof(Resource))]
    [Required]
    public long Personnummer { get; set; }

    [Display(Name = "Anställningsdatum", ResourceType = typeof(Resource))]
    [Required]
    public DateTime EmploymentDate { get; set; }

    [Display(Name = "Arbetsdagstart", ResourceType = typeof(Resource))]
    [Required]
    public TimeSpan StartWorkTime { get; set; }

    [Display(Name = "Arbetsdagslut", ResourceType = typeof(Resource))]
    [Required]
    public TimeSpan EndWorkTime { get; set; }

    [Display(Name = "Ort", ResourceType = typeof(Resource))]
    [MaxLength(30)]
    [Required]
    public string LivingPlace { get; set; } = null!;

    [Display(Name = "Telefon", ResourceType = typeof(Resource))]
    [MaxLength(15)]
    [Required]
    public string Phone { get; set; } = null!;

    public virtual List<OrderModel> Orders { get; set; } = null!;

    //public int Id { get; set; }


    //[DisplayName("Namn")]
    //[MaxLength(30)]
    //[Required]
    //public string FullName { get; set; } = null!;

    //[DisplayName("Personnummer")]
    //[Required]
    //public long Personnummer { get; set; }




    //[DisplayName("Anställnings datum")]
    //[Required]

    //public DateTime EmploymentDate { get; set; }

    //[DisplayName("Arbetsdag start")]
    //[Required]
    //public TimeSpan StartWorkTime { get; set; }
    //[DisplayName("Arbetsdag slut")]
    //[Required]
    //public TimeSpan EndWorkTime { get; set; }
    //[DisplayName("Ort")]
    //[MaxLength(30)]
    //[Required]
    //public string LivingPlace { get; set; } = null!;
    //[DisplayName("Telefon")]
    //[MaxLength(15)]
    //[Required]
    //public string Phone { get; set; } = null!;
    //public virtual List<OrderModel> Orders { get; set; } = null!;
}
