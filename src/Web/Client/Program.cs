using BlazorIdentity.Web.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]!;

builder.Services.AddApiClients(apiBaseUrl);

await builder.Build().RunAsync();
