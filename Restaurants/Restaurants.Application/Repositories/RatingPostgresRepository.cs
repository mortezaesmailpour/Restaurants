using Dapper;
using Restaurants.Application.Database;
using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

public class RatingPostgresRepository(IDbConnectionFactory dbConnectionFactory) : IRatingRepository
{
    public async Task<bool> RateRestaurantAsync(Guid restaurantId, int rating, Guid userId, CancellationToken token = default)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var result =  await connection.ExecuteAsync(new CommandDefinition("""
            insert into ratings (userid, restaurantid, rating)
            values (@userId, @restaurantId, @rating)
            on conflict (userid, restaurantid) 
                do update set rating = @rating
            """, new { userId, restaurantId , rating }, cancellationToken: token));

        return result > 0;
    }

    public async Task<float?> GetRatingAsync(Guid restaurantId, CancellationToken token = default)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
            select round(avg(r.rating),1) 
            from rating
            where restaurantid = @restaurantId
            """, new { restaurantId }, cancellationToken: token));
    }

    public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid restaurantId, Guid? userId, CancellationToken token = default)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
            select round(avg(r.rating),1),
                (select rating 
                from ratings 
                where restaurantid = @resturantId and userid = @userId 
                limit 1)    
            from rating
            where restaurantid = @RestaurantId
            """, new { restaurantId, userId }, cancellationToken: token));
    }

    public async Task<bool> DeleteRatingAsync(Guid restaurantId, Guid userId, CancellationToken token = default)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete 
            from ratings 
            where restaurantid = @restaurantId and userid = @userId
            """, new { restaurantId, userId }, cancellationToken: token));

        return result > 0;
    }

    public async Task<IEnumerable<RestaurantRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QueryAsync<RestaurantRating>(new CommandDefinition("""
            select ra.rating, ra.restaurantid
            from ratings ra
            inner join restaurants r on ra.restaurantid = r.id
            where userid = @userId
            """, new { userId }, cancellationToken: token));
    }
}