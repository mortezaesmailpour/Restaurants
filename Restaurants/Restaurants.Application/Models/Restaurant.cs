namespace Restaurants.Application.Models;

public class Restaurant
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required int YearStarted { get; set; } 
    public required List<string> Features { get; init; } = [];

}
