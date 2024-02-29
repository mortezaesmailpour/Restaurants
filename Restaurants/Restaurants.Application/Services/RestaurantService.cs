using FluentValidation;
using Restaurants.Application.Models;
using Restaurants.Application.Repositories;

namespace Restaurants.Application.Services;

public class RestaurantService(IRestaurantRepository restaurantRepository, IValidator<Restaurant> restaurantValidator) : IRestaurantService
{
    public  async Task<bool> CreateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        await restaurantValidator.ValidateAndThrowAsync(restaurant, cancellationToken: token);
        return await restaurantRepository.CreateAsync(restaurant, token);
    }

    public Task<Restaurant?> GetByIdAsync(Guid id, CancellationToken token = default) 
        => restaurantRepository.GetByIdAsync(id, token);

    public Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken token = default) 
        => restaurantRepository.GetAllAsync(token);

    public async Task<Restaurant?> UpdateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        await restaurantValidator.ValidateAndThrowAsync(restaurant, cancellationToken: token);
        var restaurantExists = await restaurantRepository.ExistsByIdAsync(restaurant.Id, token);
        if (!restaurantExists)
            return null;
        await restaurantRepository.UpdateAsync(restaurant, token);
        return restaurant;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token = default) 
        => restaurantRepository.DeleteAsync(id, token);
}
