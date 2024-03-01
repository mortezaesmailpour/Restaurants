using FluentValidation;
using Restaurants.Application.Models;
using Restaurants.Application.Repositories;

namespace Restaurants.Application.Services;

public class RestaurantService(
    IRestaurantRepository restaurantRepository, 
    IRatingRepository ratingRepository,
    IValidator<Restaurant> restaurantValidator) : IRestaurantService
{
    public  async Task<bool> CreateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        await restaurantValidator.ValidateAndThrowAsync(restaurant, cancellationToken: token);
        return await restaurantRepository.CreateAsync(restaurant, token);
    }

    public Task<Restaurant?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default) 
        => restaurantRepository.GetByIdAsync(id, userId, token);

    public Task<IEnumerable<Restaurant>> GetAllAsync(Guid? userId = default,CancellationToken token = default) 
        => restaurantRepository.GetAllAsync(userId, token);

    public async Task<Restaurant?> UpdateAsync(Restaurant restaurant, Guid? userId = default, CancellationToken token = default)
    {
        await restaurantValidator.ValidateAndThrowAsync(restaurant, cancellationToken: token);
        var restaurantExists = await restaurantRepository.ExistsByIdAsync(restaurant.Id, token);
        if (!restaurantExists)
            return null;
        await restaurantRepository.UpdateAsync(restaurant, token);
        if (userId.HasValue)
            (restaurant.Rating, restaurant.UserRating) = await ratingRepository.GetRatingAsync(restaurant.Id, userId.Value, token);
        else
            restaurant.Rating = await ratingRepository.GetRatingAsync(restaurant.Id, token);
        return restaurant;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token = default) 
        => restaurantRepository.DeleteAsync(id, token);
}