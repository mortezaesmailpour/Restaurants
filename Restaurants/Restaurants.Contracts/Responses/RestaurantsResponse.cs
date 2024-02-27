namespace Restaurants.Contracts.Responses;

public class RestaurantsResponse
{
    public required IEnumerable<RestaurantResponse> Items { get; init; } = Enumerable.Empty<RestaurantResponse>();
}
