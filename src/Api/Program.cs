using BlazorIdentity.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("api",
        policy =>
        {
            policy.WithOrigins("https://localhost:7113")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("api");

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
    return context.Response.WriteAsJsonAsync(context.User.Claims.Select(c => new { c.Type, c.Value }));
});

app.Run();
