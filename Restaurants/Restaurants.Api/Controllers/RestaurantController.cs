using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Mapping;
using Restaurants.Application.Repositories;
using Restaurants.Contracts.Requests;

namespace Restaurants.Api.Controllers;

[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantController(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    [HttpPost(ApiEndpoints.Restaurant.Create)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request)
    {
        var restaurant = request.MapToRestaurant();
        await _restaurantRepository.CreateAsync(restaurant);
        var response = restaurant.MapToResponse();
        return CreatedAtAction(nameof(Get), new { id = restaurant.Id }, response);
        //return Created($"{ApiEndpoints.Restaurant.Create}/{restaurant.Id}", response);
    }

    [HttpGet(ApiEndpoints.Restaurant.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(id);
        if (restaurant is null)
            return NotFound();
        var response = restaurant.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Restaurant.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _restaurantRepository.GetAllAsync();
        var response = restaurants.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Restaurant.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRestaurantRequest request)
    {
        var restaurant = request.MapToRestaurant(id);
        var updated = await _restaurantRepository.UpdateAsynce(restaurant);
        if (!updated)
            return NotFound();
        var response = restaurant.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Restaurant.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _restaurantRepository.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return Ok();
    }
}


