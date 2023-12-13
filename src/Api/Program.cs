using System.Security.Claims;
using BlazorIdentity.Relational;
using BlazorIdentity.Shared.Models;
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherForecasts", context =>
{
    var forecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]))
        .ToArray();

    return context.Response.WriteAsJsonAsync(forecasts);
});

app.MapGet("/me", context =>
{
    return context.Response.WriteAsJsonAsync(context.User.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)));
}).RequireAuthorization().RequireCors("api");

app.UseCors("api");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
