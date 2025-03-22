using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nordic_Align_V7;
using Nordic_Align_V7.Models;
using Nordic_Align_V7.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register NordicAlignDBContext
builder.Services.AddDbContext<NordicAlignDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NordicDBConnectionString")));

// Register InvoiceDBContext
builder.Services.AddDbContext<InvoiceDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InvoiceDBConnectionString")));

// Add Identity services
builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<NordicAlignDBContext>()
    .AddDefaultTokenProviders();

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<IMailService, MailService>();




var app = builder.Build();
// Create roles 
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRolesAsync(roleManager);
}



// Enable QuestPDF Community License
QuestPDF.Settings.License = LicenseType.Community;



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Seed roles 
async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string adminRole = "Admin";
    string userRole = "User";

    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    if (!await roleManager.RoleExistsAsync(userRole))
    {
        await roleManager.CreateAsync(new IdentityRole(userRole));
    }

}