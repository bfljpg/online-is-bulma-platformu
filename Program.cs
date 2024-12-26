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
builder.Services.AddDistributedMemoryCache(); // Session i�in gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum s�resi
    options.Cookie.HttpOnly = true; // G�venlik amac�yla HttpOnly
    options.Cookie.IsEssential = true; // GDPR uyumlulu�u i�in gerekli
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
app.UseSession(); // Session middleware burada �a�r�lmal�
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
