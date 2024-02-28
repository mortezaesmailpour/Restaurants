using Restaurants.Application.Models;

namespace Restaurants.Application.Services;

public interface IRestaurantService
{
    Task<bool> CreateAsync(Restaurant restaurant);
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> UpdateAsync(Restaurant restaurant);
    Task<bool> DeleteAsync(Guid id);
}
