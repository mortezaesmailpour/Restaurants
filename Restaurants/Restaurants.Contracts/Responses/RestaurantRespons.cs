namespace Restaurants.Contracts.Responses;

public class RestaurantRespons
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }

    //public required string Address { get; init; }

    //public required string Type { get; init; } // "Restaurant", "Cafe", "Bakery", etc.

    public required int YearStarted { get; init; } // Year the establishment was started

    public required IEnumerable<string> Features { get; init; } = Enumerable.Empty<string>();
}
