using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Database;
using Restaurants.Application.Repositories;
using Restaurants.Application.Services;
using Restaurants.Application.Validators;

namespace Restaurants.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRatingRepository,RatingPostgresRepository>();
        services.AddSingleton<IRatingService, RatingService>();
        services.AddSingleton<IRestaurantRepository, RestaurantPostgresRepository>();
        services.AddSingleton<IRestaurantService, RestaurantService>();
        services.AddValidatorsFromAssemblyContaining<RestaurantValidator>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}