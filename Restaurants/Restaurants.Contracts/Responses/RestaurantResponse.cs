namespace Restaurants.Contracts.Responses;

public class RestaurantResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public float? Rating { get; init; }
    public int? UserRating { get; init; }
    public required int YearStarted { get; init; } 
    public required IEnumerable<string> Features { get; init; } = Enumerable.Empty<string>();
}