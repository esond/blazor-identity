using System.Security.Claims;
using BlazorIdentity.Api;
using BlazorIdentity.Relational;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("api",
        policy =>
        {
            policy.WithOrigins("https://localhost:7192", "https://localhost:5277")
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorizationBuilder();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapIdentityApi<ApplicationUser>();

// provide an end point to clear the cookie for logout
// NOTE: This logout code will be updated shortly.
//       https://github.com/dotnet/blazor-samples/issues/132
app.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return TypedResults.Ok();
});

app.MapWeatherForecastsEndpoint()
    .MapMeEndpoint();

app.UseCors("api");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
