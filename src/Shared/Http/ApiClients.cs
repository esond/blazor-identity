using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorIdentity.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
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
