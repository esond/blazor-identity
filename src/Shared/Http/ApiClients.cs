using BlazorIdentity.Shared.Models;
using Refit;

namespace BlazorIdentity.Shared.Http;

public interface IWeatherApi
{
    [Get("/weatherForecasts")]
    Task<IApiResponse<WeatherForecast[]>> GetWeatherForecasts();
}

public interface IUsersApi
{
    [Get("/me")]
    Task<IApiResponse<KeyValuePair<string, string>[]>> GetMe();
}
