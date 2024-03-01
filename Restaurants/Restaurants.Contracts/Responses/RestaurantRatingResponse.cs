namespace Restaurants.Contracts.Responses;

public class RestaurantRatingResponse
{
    public required Guid RestaurantId { get; init; }
    public required int Rating { get; init; }
}
