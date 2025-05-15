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
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.Use(async (context, next) =>
{
    string cookie = string.Empty;
    if (context.Request.Cookies.TryGetValue("Language", out cookie!))
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie);
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie);

    }
    else
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");

    }
    await next.Invoke();
});
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