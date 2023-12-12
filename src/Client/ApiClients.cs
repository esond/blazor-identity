using BlazorIdentity.Shared.Models;
using Refit;

namespace BlazorIdentity.Web.Client;

public interface IWeatherApi
{
    [Get("/weatherForecasts")]
    Task<IApiResponse<WeatherForecast[]>> GetWeatherForecasts();
}

public interface IUsersApi
{
    [Get("/me")]
    Task<IApiResponse<(string claimType, string claimValue)[]>> GetMe();
}
