using BlazorIdentity.Web.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddRefitClient<IWeatherApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://localhost:5277"));

await builder.Build().RunAsync();
