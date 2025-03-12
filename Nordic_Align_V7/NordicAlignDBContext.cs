using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7.Models;

namespace Nordic_Align_V7;

public class NordicAlignDBContext : IdentityDbContext<UserModel>
{
    private readonly IConfiguration _configuration;

    public NordicAlignDBContext(DbContextOptions<NordicAlignDBContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<CourierModel> Couriers { get; set; }
    public virtual DbSet<OrderModel> Orders { get; set; }
    public virtual DbSet<RecepientModel> Recepients { get; set; }
    public virtual DbSet<TranportModel> Transports { get; set; }
    public virtual DbSet<WarehouseModel> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("NordicDBConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderModel>()
            .Navigation(o => o.Recepient)
            .AutoInclude();

        modelBuilder.Entity<OrderModel>()
            .Navigation(o => o.Courier)
            .AutoInclude();

        modelBuilder.Entity<OrderModel>()
            .Navigation(o => o.Transport)
            .AutoInclude();

        modelBuilder.Entity<OrderModel>()
            .Navigation(o => o.Warehouses)
            .AutoInclude();

        modelBuilder.Entity<OrderModel>()
            .Property(o => o.Price)
            .HasDefaultValue(100);
    }
}
