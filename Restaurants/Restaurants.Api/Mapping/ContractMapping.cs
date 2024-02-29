using Restaurants.Application.Models;
using Restaurants.Contracts.Requests;
using Restaurants.Contracts.Responses;

namespace Restaurants.Api.Mapping;

public static class ContractMapping
{
    public static Restaurant MapToRestaurant(this CreateRestaurantRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            YearStarted = request.YearStarted,
            Features = request.Features.ToList(),
        };
    public static Restaurant MapToRestaurant(this UpdateRestaurantRequest request, Guid id)
        => new()
        {
            Id = id,
            Name = request.Name,
            YearStarted = request.YearStarted,
            Features = request.Features.ToList(),
        };
    public static RestaurantResponse MapToResponse(this Restaurant restaurant)
        => new()
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            YearStarted = restaurant.YearStarted,
            Features = restaurant.Features,
        };
    public static RestaurantsResponse MapToResponse(this IEnumerable<Restaurant> restaurants)
        => new()
        {
            Items = restaurants.Select(MapToResponse)
        };
}
