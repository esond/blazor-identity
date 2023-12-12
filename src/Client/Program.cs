using BlazorIdentity.Web.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

var apiBaseUri = new Uri("https://localhost:5277");

builder.Services.AddRefitClient<IWeatherApi>()
    .ConfigureHttpClient(client => client.BaseAddress = apiBaseUri);

builder.Services.AddRefitClient<IUsersApi>()
    .ConfigureHttpClient(client => client.BaseAddress = apiBaseUri)
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<CookieHandler>();

await builder.Build().RunAsync();
