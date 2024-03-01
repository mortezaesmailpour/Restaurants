using FluentValidation;
using FluentValidation.Results;
using Restaurants.Application.Models;
using Restaurants.Application.Repositories;

namespace Restaurants.Application.Services;

public class RatingService(IRatingRepository ratingRepository, IRestaurantRepository restaurantRepository) : IRatingService
{
    public async Task<bool> RateRestaurantAsync(Guid restaurantId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating is <= 0 or >5)
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = nameof(rating),
                    ErrorMessage = "Rating must be between 1 and 5"
                }
            });

        var restaurantExists = await restaurantRepository.ExistsByIdAsync(restaurantId, token);
        if (!restaurantExists)
            return false;
        return await ratingRepository.RateRestaurantAsync(restaurantId, rating, userId, token);
    }
    public Task<bool> DeleteRatingAsync(Guid restaurantId, Guid userId, CancellationToken token = default)
        => ratingRepository.DeleteRatingAsync(restaurantId,userId, token);

    public Task<IEnumerable<RestaurantRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        => ratingRepository.GetRatingsForUserAsync(userId, token);
}