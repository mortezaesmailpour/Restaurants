using Restaurants.Application.Models;

namespace Restaurants.Application.Services;

public interface IRatingService
{
    Task<bool> RateRestaurantAsync(Guid restaurantId, int rating, Guid userId, CancellationToken token = default);
    Task<bool> DeleteRatingAsync(Guid restaurantId, Guid userId, CancellationToken token = default);
    Task<IEnumerable<RestaurantRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
}
