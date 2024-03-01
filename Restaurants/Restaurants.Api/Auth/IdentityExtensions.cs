﻿namespace Restaurants.Api.Auth;

public static class IdentityExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x => x.Type == "userid");
        return Guid.TryParse(userId?.Value, out var Id) ? Id : null;
    }
}