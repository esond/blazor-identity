using BlazorIdentity.Shared.Models;

namespace BlazorIdentity.Api;

public static class ProgramExtensions
{
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
