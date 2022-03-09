using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMoscow.DataAccess.Interfaces;
using WeatherMoscow.DataAccess.Services;
using WeatherMoscow.Entity.DbContext;

namespace WeatherMoscow.Infrastructure;

public static class DataModule
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddTransient(typeof(IRepository<>), typeof(MSSqlDBRepository<>));
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("WeatherMoscow"));
        });
        
        return services;
    }
}