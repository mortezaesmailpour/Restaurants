using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Mapping;
using Restaurants.Application.Services;
using Restaurants.Contracts.Requests;

namespace Restaurants.Api.Controllers;

[ApiController]
public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Restaurant.Create)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request)
    {
        var restaurant = request.MapToRestaurant();
        await restaurantService.CreateAsync(restaurant);
        var response = restaurant.MapToResponse();
        return CreatedAtAction(nameof(Get), new { id = restaurant.Id }, response);
        //return Created($"{ApiEndpoints.Restaurant.Create}/{restaurant.Id}", response);
    }

    [HttpGet(ApiEndpoints.Restaurant.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var restaurant = await restaurantService.GetByIdAsync(id);
        if (restaurant is null)
            return NotFound();
        var response = restaurant.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Restaurant.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantService.GetAllAsync();
        var response = restaurants.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Restaurant.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRestaurantRequest request)
    {
        var restaurant = request.MapToRestaurant(id);
        var updatedRestaurant = await restaurantService.UpdateAsync(restaurant);
        if (updatedRestaurant is null)
            return NotFound();
        var response = restaurant.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Restaurant.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await restaurantService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return Ok();
    }
}


