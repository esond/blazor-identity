using BlazorIdentity.Shared.Http;
using Refit;

namespace BlazorIdentity.Web.Client;

public static class ProgramExtensions
{
    public static IServiceCollection AddApiClients(this IServiceCollection services, string baseUrl)
    {
        services.AddTransient<CookieHandler>();

        services.AddRefitClient<IWeatherApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrl))
            .AddHttpMessageHandler<CookieHandler>();

        services.AddRefitClient<IUsersApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseUrl))
            .AddHttpMessageHandler<CookieHandler>();

        return services;
    }


}
