using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

public interface IRestaurantRepository
{
    Task<bool> CreateAsync(Restaurant restaurant); 
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<bool> UpdateAsync(Restaurant restaurant);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
}
