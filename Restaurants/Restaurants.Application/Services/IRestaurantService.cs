using Restaurants.Application.Models;

namespace Restaurants.Application.Services;

public interface IRestaurantService
{
    Task<bool> CreateAsync(Restaurant restaurant, CancellationToken token = default);
    Task<Restaurant?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken token = default);
    Task<Restaurant?> UpdateAsync(Restaurant restaurant, CancellationToken token = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
}
