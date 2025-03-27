using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Nordic_Align_V7;

public class InvoiceDBContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserModel> _userManager;

    public InvoiceDBContext(DbContextOptions<InvoiceDBContext> options, IConfiguration configuration, UserManager<UserModel> userManager)
        : base(options)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public InvoiceDBContext(DbContextOptions<InvoiceDBContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }




    public virtual DbSet<AddressModel> Addresses { get; set; }
    public virtual DbSet<InvoiceModel> Invoices { get; set; }
    public virtual DbSet<InvoiceItemModel> InvoiceItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("InvoiceDBConnectionString");
            optionsBuilder.UseSqlServer(connectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<InvoiceItemModel>()
      .Property(i => i.Price)
      .HasPrecision(18, 2);


        modelBuilder.Entity<InvoiceModel>()
            .HasMany(i => i.Items)
            .WithOne(ii => ii.Invoice)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }  

}
