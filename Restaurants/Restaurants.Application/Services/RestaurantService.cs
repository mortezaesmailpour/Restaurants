using FluentValidation;
using Restaurants.Application.Models;
using Restaurants.Application.Repositories;

namespace Restaurants.Application.Services;

public class RestaurantService(IRestaurantRepository restaurantRepository, IValidator<Restaurant> restaurantValidator) : IRestaurantService
{
    public  async Task<bool> CreateAsync(Restaurant restaurant)
    {
        await restaurantValidator.ValidateAndThrowAsync(restaurant);
        return await restaurantRepository.CreateAsync(restaurant);
    }

    public Task<Restaurant?> GetByIdAsync(Guid id)
    {
        return restaurantRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return restaurantRepository.GetAllAsync();
    }

    public async Task<Restaurant?> UpdateAsync(Restaurant restaurant)
    {
        await restaurantValidator.ValidateAndThrowAsync(restaurant);
        var restaurantExists = await restaurantRepository.ExistsByIdAsync(restaurant.Id);
        if (!restaurantExists)
            return null;
        await restaurantRepository.UpdateAsync(restaurant);
        return restaurant;
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return restaurantRepository.DeleteAsync(id);
    }
}
