using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Auth;
using Restaurants.Api.Mapping;
using Restaurants.Application.Services;
using Restaurants.Contracts.Requests;

namespace Restaurants.Api.Controllers;

[Authorize]
[ApiController]
public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
{
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Restaurant.Create)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken token)
    {
        var restaurant = request.MapToRestaurant();
        if (!await restaurantService.CreateAsync(restaurant, token))
            return BadRequest();
        var response = restaurant.MapToResponse();
        return CreatedAtAction(nameof(Get), new { id = restaurant.Id }, response);
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpGet(ApiEndpoints.Restaurant.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var restaurant = await restaurantService.GetByIdAsync(id, userId,token);
        if (restaurant is null)
            return NotFound();
        return Ok(restaurant.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Restaurant.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var restaurants = await restaurantService.GetAllAsync(userId, token);
        return Ok(restaurants.MapToResponse());
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Restaurant.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRestaurantRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var restaurant = request.MapToRestaurant(id);
        var updatedRestaurant = await restaurantService.UpdateAsync(restaurant, userId, token);
        if (updatedRestaurant is null)
            return NotFound();
        return Ok(restaurant.MapToResponse());
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Restaurant.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        => await restaurantService.DeleteAsync(id, token) ? Ok() : NotFound();
}