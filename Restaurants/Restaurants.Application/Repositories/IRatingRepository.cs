using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

public interface IRatingRepository
{
    Task<bool> RateRestaurantAsync(Guid restaurantId, int rating, Guid userId, CancellationToken token = default);
    Task<float?> GetRatingAsync(Guid restaurantId, CancellationToken token = default);
    Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid restaurantId, Guid? userId , CancellationToken token = default);
    Task<bool> DeleteRatingAsync(Guid restaurantId, Guid userId , CancellationToken token = default);
    Task<IEnumerable<RestaurantRating>> GetRatingsForUserAsync(Guid userId , CancellationToken token = default);
}