namespace Restaurants.Contracts.Requests;

public class UpdateRestaurantRequest
{
    public required string Name { get; init; }
    public required int YearStarted { get; init; } // Year the establishment was started
    public required IEnumerable<string> Features { get; init; } = Enumerable.Empty<string>();
}