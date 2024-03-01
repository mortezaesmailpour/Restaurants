using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Auth;
using Restaurants.Api.Mapping;
using Restaurants.Application.Services;
using Restaurants.Contracts.Requests;

namespace Restaurants.Api.Controllers;

[ApiController]
public class RstingController(IRatingService ratingService): ControllerBase
{
    [Authorize]
    [HttpPut(ApiEndpoints.Restaurant.Rate)]
    public async Task<IActionResult> RateRestaurant([FromRoute] Guid id, [FromBody] RateRestaurantRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await ratingService.RateRestaurantAsync(id, request.Rating, userId!.Value, token);
        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpDelete(ApiEndpoints.Restaurant.DeleteRating)]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        return await ratingService.DeleteRatingAsync(id, userId!.Value, token) ? Ok() : NotFound();
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    public async Task<IActionResult> GetUserRating(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var ratings = await ratingService.GetRatingsForUserAsync( userId!.Value, token);
        return Ok(ratings.MapToResponse());
    }
}