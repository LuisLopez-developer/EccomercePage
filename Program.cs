using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EccomercePage;
using Microsoft.AspNetCore.Components.Authorization;
using EccomercePage.Api.Services.AccountService;
using EccomercePage.Api.Services.ProductServices;
using EccomercePage.Api.Interfaces.AccountInterface;
using EccomercePage.Api.Interfaces.ProductInterfaces;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar servicios de autorización y autenticación
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// Resto de servicios
builder.Services.AddScoped<IProduct, ProductService>();
builder.Services.AddScoped<IProductBrand, ProductBrandService>();
builder.Services.AddScoped<IProductCategory, ProductCategoryService>();

// Configura un HttpClient con un tiempo de vida "scoped"
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetSection("Url")["FrontendUrl"] ?? "https://localhost:7040") // El BaseAddress se obtiene de la configuración o se establece en un valor predeterminado
});

// Configura un HttpClient con nombre "Auth"
builder.Services.AddHttpClient("Auth", opt => opt.BaseAddress =
new Uri(builder.Configuration.GetSection("Url")["BackendUrl"] ?? "https://localhost:7239")) // El BaseAddress se obtiene de la configuración o se establece en un valor predeterminado
    .AddHttpMessageHandler<CustomHttpHandler>(); // Añade un HttpMessageHandler personalizado para este cliente

await builder.Build().RunAsync();
