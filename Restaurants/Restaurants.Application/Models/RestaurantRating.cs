namespace Restaurants.Application.Models;

public class RestaurantRating
{
    public required Guid RestaurantId { get; init; }
    public required int Rating { get; init; }
}