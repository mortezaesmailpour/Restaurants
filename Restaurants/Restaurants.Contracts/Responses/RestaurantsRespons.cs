namespace Restaurants.Contracts.Responses;

public class RestaurantsRespons
{
    public required IEnumerable<RestaurantRespons> Items { get; init; } = Enumerable.Empty<RestaurantRespons>();
}
