using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

internal class RestaurantRepository : IRestaurantRepository
{
    private readonly List<Restaurant> _restaurants = [];
    public Task<bool> CreateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        _restaurants.Add(restaurant);
        return Task.FromResult(true);
    }

    public Task<Restaurant?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default) 
        => Task.FromResult(_restaurants.FirstOrDefault(x => x.Id == id));

    public Task<IEnumerable<Restaurant>> GetAllAsync(Guid? userId = default, CancellationToken token = default) 
        => Task.FromResult(_restaurants.AsEnumerable());

    public Task<bool> UpdateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        var restaurantIndex = _restaurants.FindIndex(x => x.Id == restaurant.Id);
        if (restaurantIndex == -1)
            return Task.FromResult(false);
        _restaurants[restaurantIndex] = restaurant;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token = default) 
        => Task.FromResult(_restaurants.RemoveAll(x => x.Id == id) > 0);

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default) 
        => Task.FromResult(_restaurants.FindIndex(x => x.Id == id) != -1);
}