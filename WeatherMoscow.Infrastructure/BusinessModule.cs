using Microsoft.Extensions.DependencyInjection;
using WeatherMoscow.Services;
using WeatherMoscow.Services.Interfaces;

namespace WeatherMoscow.Infrastructure;

public static class BusinessModule
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddTransient<IWeatherService, WeatherService>();
        return services;
    }
}