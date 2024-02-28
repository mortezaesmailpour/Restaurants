using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

internal class RestaurantRepository : IRestaurantRepository
{
    private readonly List<Restaurant> _restaurants = [];
    public Task<bool> CreateAsync(Restaurant restaurant)
    {
        _restaurants.Add(restaurant);
        return Task.FromResult(true);
    }

    public Task<Restaurant?> GetByIdAsync(Guid id)
    {
        var restaurant = _restaurants.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(restaurant);
    }

    public Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return Task.FromResult(_restaurants.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Restaurant restaurant)
    {
        var restaurantIndex = _restaurants.FindIndex(x => x.Id == restaurant.Id);
        if (restaurantIndex == -1)
            return Task.FromResult(false);
        _restaurants[restaurantIndex] = restaurant;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var removedCount = _restaurants.RemoveAll(x => x.Id == id);
        return Task.FromResult(removedCount > 0);
    }

    public Task<bool> ExistsByIdAsync(Guid id)
    {
        var restaurantIndex = _restaurants.FindIndex(x => x.Id == id);
        return Task.FromResult(restaurantIndex != -1);
    }
}
