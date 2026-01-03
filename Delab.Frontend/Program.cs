using Delab.AccessService.Repositories;
using Delab.Frontend;
using Delab.Frontend.AuthenticationProviders;
using Delab.Frontend.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuraci�n del HttpClient para la comunicaci�n con el backend
// Usa la URL actual (será el mismo servidor en producción)
var baseAddress = builder.HostEnvironment.IsProduction() 
    ? builder.HostEnvironment.BaseAddress 
    : "https://localhost:7254";
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

// Servivios mudBlazor
builder.Services.AddMudServices();
//Sistema de Seguridad
builder.Services.AddAuthorizationCore();

// Reemplazar la configuraci�n del IRepository
builder.Services.AddScoped(sp =>
{
    var jsRuntime = sp.GetRequiredService<IJSRuntime>(); // Obtener el IJSRuntime
    var httpClient = sp.GetRequiredService<HttpClient>(); ;

    // Usar tu clase de extensi�n para obtener el token desde localStorage
    return new Repository(
        httpClient,
        async () =>
        {
            var token = await jsRuntime.GetLocalStorage("TOKEN_KEY");
            return Convert.ToString(token); // Asegurarse de que el token es un string
        }
    );
});

// Registrar IRepository como Repository
builder.Services.AddScoped<IRepository>(sp => sp.GetRequiredService<Repository>());

//Authentication Provider
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

await builder.Build().RunAsync();