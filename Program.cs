using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EccomercePage;
using Microsoft.AspNetCore.Components.Authorization;
using EccomercePage.Services;
using EccomercePage.Interfaces;
using EccomercePage.Interfaces.ProductInterfaces;
using EccomercePage.Services.ProductServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<CutomHttpHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// Resto de servicios
builder.Services.AddScoped<IProduct, ProductService>();

// Configura un HttpClient con un tiempo de vida "scoped"
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetSection("Url")["FrontendUrl"] ?? "https://localhost:7040") // El BaseAddress se obtiene de la configuración o se establece en un valor predeterminado
});

// Configura un HttpClient con nombre "Auth"
builder.Services.AddHttpClient("Auth", opt => opt.BaseAddress =
new Uri(builder.Configuration.GetSection("Url")["BackendUrl"] ?? "https://localhost:7239")) // El BaseAddress se obtiene de la configuración o se establece en un valor predeterminado
    .AddHttpMessageHandler<CutomHttpHandler>(); // Añade un HttpMessageHandler personalizado para este cliente

await builder.Build().RunAsync();
