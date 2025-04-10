using ETicaretDal.Abstract;
using ETicaretDal.Concrete;
using ETicaretData.Context;
using ETicaretData.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Dependency Injection
builder.Services.AddDbContext<ETicaretContext>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();
builder.Services.AddScoped<IProductDal, ProductDal>();
builder.Services.AddScoped<IOrderDal, OrderDal>();
builder.Services.AddScoped<IOrderLineDal, OrderLineDal>();
builder.Services.AddScoped<ISupplierDal, SupplierDal>();


//var app = builder.Build();

// Identity (Kimlik) Dogrulama
builder.Services.AddIdentity<AppUser, AppRole>(option =>
{
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Kilitleme Suresi
    option.Lockout.MaxFailedAccessAttempts = 5; // Max basarisiz giris
    option.Password.RequireDigit = false; // Rakam zorunlulugu kaldirma
    option.Password.RequireNonAlphanumeric = false; // Ozel karakter zorunlulugu kaldirma
    option.Password.RequireLowercase = false; // Kucuk harf zorunlulugunu kaldirma
    option.Password.RequireUppercase = false; // Buyuk harf zorunlulugunu kaldirma
}).AddEntityFrameworkStores<ETicaretContext>() // EF ile veri tabanı baglantisini saglar
.AddDefaultTokenProviders(); // Varsayilan token saglayicilarini ekler


// Cerezler (Cookies)
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Account/Login"; // Giris yapilmadiginda yonlendirilmesi gereken sayfa
    option.AccessDeniedPath = "/"; // Yetkisiz erisim oldugunda yonlendirilecek sayfa
    option.Cookie = new CookieBuilder()
    {
        Name = "AspNetCoreIdentityExampleCookie", // Cerez ismi
        HttpOnly = false, // Cerez http
        SameSite = SameSiteMode.Lax, // Cerez ayni sitede yapilacak isteklerde gecerli
        SecurePolicy = CookieSecurePolicy.Always // Cerez sadece SSL sertifikasi olan istekler uzerinden iletilir
    };
    option.SlidingExpiration = true; // Cerez gecerlilik suresi doldukca yenilenir
    option.ExpireTimeSpan = TimeSpan.FromMinutes(15); // Cerez gecerlilik suresi
});


// Oturum
builder.Services.AddSession(); // Oturum yonetim servisi
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // http den https yonlendirilir
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik dogrulama islemleri
app.UseAuthorization(); // Yetkilendirme islemleri
app.UseSession(); // Oturum yonetimini aktiflestirir

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=List}/{id?}");

app.Run();

