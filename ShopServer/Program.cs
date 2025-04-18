using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopServer.Services.Extentions; // Extension metodlar için
using ShopServer.Services.Infrastuce;
using ShopServer.Services.Services;
using ShopSharedLibrary.DBContextOperation.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Uygulama Konfigürasyonu
// ----------------------------
var configuration = builder.Configuration; // Uygulama yapılandırma ayarları

// 2️⃣ MVC ve Razor Pages Servisleri
// --------------------------------
builder.Services.AddControllersWithViews(); // MVC Controller desteği
builder.Services.AddRazorPages(); // Razor Pages desteği

// 3️⃣ Veritabanı Bağlantısı
// ------------------------
builder.Services.AddDbContext<BlazorShopDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); // SQL Server bağlantısı

// 4️⃣ JWT Kimlik Doğrulama Ayarları
// --------------------------------
builder.Services.AddAuthentication(options =>
{
    // Varsayılan kimlik doğrulama şemalarını ayarla
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Token doğrulama parametrelerini yapılandır
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Token yayıncısını doğrula
        ValidateAudience = true, // Hedef kitleyi doğrula
        ValidateLifetime = true, // Token süresini doğrula
        ValidateIssuerSigningKey = true, // İmza anahtarını doğrula
        ValidIssuer = configuration["JwtIssuer"], // Geçerli yayıncı
        ValidAudience = configuration["JwtAudience"], // Geçerli hedef kitle
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"])) // Güvenlik anahtarı
    };
});

// 5️⃣ Özel Servislerin Kaydı
// --------------------------
builder.Services.AddScoped<ShopClient.ModalManager>(); // Modal yönetimi servisi (KONTROL EDİLECEK)
builder.Services.ConfigureMapping(); // AutoMapper konfigürasyonu (Extension metot)
builder.Services.AddScoped<IUserService, UserService>(); // Kullanıcı servisi

// 6️⃣ API Dökümantasyonu
// ---------------------
builder.Services.AddEndpointsApiExplorer(); // API endpoint keşfi
builder.Services.AddSwaggerGen(); // Swagger dökümantasyonu

// 7️⃣ Tarayıcı Yerel Depolama
// --------------------------
builder.Services.AddBlazoredLocalStorage(conf =>
    conf.JsonSerializerOptions.WriteIndented = true); // Blazor için yerel depolama servisi

// 8️⃣ Uygulama Yapılandırması
// ---------------------------
var app = builder.Build();

// 9️⃣ Geliştirme Ortamı Middleware'leri
// ------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger JSON endpoint'ini etkinleştir
    app.UseSwaggerUI(); // Swagger UI arayüzünü etkinleştir
    app.UseWebAssemblyDebugging(); // Blazor WebAssembly hata ayıklama
}
else
{
    app.UseExceptionHandler("/Error"); // Production hata yönetimi
    app.UseHsts(); // HTTP Strict Transport Security
}

// 🔟 Temel Middleware'ler
// -----------------------
app.UseHttpsRedirection(); // HTTPS yönlendirmesi
app.UseBlazorFrameworkFiles(); // Blazor WebAssembly dosyaları
app.UseStaticFiles(); // Statik dosya sunumu
app.UseRouting(); // Routing middleware

// 1️⃣1️⃣ Güvenlik Middleware'leri
// ------------------------------
app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization(); // Yetkilendirme

// 1️⃣2️⃣ Endpoint Yapılandırması
// ----------------------------
app.MapRazorPages(); // Razor Pages endpoint'leri
app.MapControllers(); // API Controller endpoint'leri
app.MapFallbackToFile("index.html"); // SPA fallback

// 1️⃣3️⃣ Uygulamayı Başlat
// -----------------------
app.Run(); // Uygulamayı çalıştır