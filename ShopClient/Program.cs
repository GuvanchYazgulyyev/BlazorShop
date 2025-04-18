using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopClient;
using ShopClient.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage(); // Local Storage için
builder.Services.AddAuthorizationCore(); // Login  için. Yani Login yapmadan sayfayı görmesin.
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
await builder.Build().RunAsync();
