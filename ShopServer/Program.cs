//using ShopServer.Services.Infrastuce;
//using ShopServer.Services.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllersWithViews(); // Degişti
//builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddRazorPages(); // Yeni
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.UseWebAssemblyDebugging(); //Yeni
//}

//app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles(); // Yeni
//app.UseStaticFiles(); // Yeni
//app.UseAuthorization();
//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html"); // Yeni
//app.Run();



using Microsoft.EntityFrameworkCore;
using ShopServer.Services.Extentions; // ✅ Extension metodun bulunduğu namespace
using ShopServer.Services.Infrastuce;
using ShopServer.Services.Services;
using ShopSharedLibrary.DBContextOperation.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 🔌 Add DbContext
builder.Services.AddDbContext<BlazorShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ✅ AutoMapper yapılandırmasını ekle (Sadece bunu kullan yeterli)
builder.Services.ConfigureMapping(); // 👈 Extension metod çağrısı

// ✅ Servis bağımlılıklarını ekle
builder.Services.AddScoped<IUserService, UserService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging(); // Blazor WebAssembly desteği
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
