using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Repositories;

namespace Restaurants.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IRestaurantRepository,RestaurantRepository>();
        return services;
    }
}
