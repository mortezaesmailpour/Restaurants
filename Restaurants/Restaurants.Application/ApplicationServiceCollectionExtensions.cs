﻿using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Database;
using Restaurants.Application.Repositories;
using Restaurants.Application.Services;

namespace Restaurants.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRestaurantRepository,RestaurantPostgresRepository>();
        services.AddSingleton<IRestaurantService, RestaurantService>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
