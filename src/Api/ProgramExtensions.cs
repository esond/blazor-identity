using BlazorIdentity.Relational;
using BlazorIdentity.Shared.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorIdentity.Api;

public static class ProgramExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddDataProtection()
            .SetApplicationName("BlazorIdentity");

        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies()
            .ApplicationCookie!.Configure(opt => opt.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
            });

        services.AddAuthorizationBuilder();

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }

    public static WebApplication MapWeatherForecastsEndpoint(this WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherForecasts", () =>
            {
                var forecasts = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast(
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]))
                    .ToArray();

                return forecasts;
            })
            .RequireAuthorization()
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        return app;
    }

    public static WebApplication MapMeEndpoint(this WebApplication app)
    {
        app.MapGet("/me", context =>
            {
                var values = context.Response.WriteAsJsonAsync(context.User.Claims.Select(c =>
                    new KeyValuePair<string, string>(c.Type, c.Value)));

                return values;
            })
            .RequireAuthorization()
            .WithName("GetMe")
            .WithOpenApi();

        return app;
    }
}
