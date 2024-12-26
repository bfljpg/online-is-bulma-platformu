using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using online_is_bulma_platformu.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("JobPortalDB");
builder.Services.AddDbContext<JobPortalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobPortalDB")));



builder.Services.AddHttpContextAccessor();

// Session ve Cache servislerini ekleyin
builder.Services.AddDistributedMemoryCache(); // Session için gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true; // Güvenlik amacýyla HttpOnly
    options.Cookie.IsEssential = true; // GDPR uyumluluðu için gerekli
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Session middleware burada çaðrýlmalý
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
